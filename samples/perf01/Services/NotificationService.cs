using System.Text.Json;

namespace Perf01.Services;

/// <summary>
/// Manages sending notifications through various channels (email, SMS, push).
/// Supports templating, batching, and retry logic.
/// </summary>
public class NotificationService
{
    public enum NotificationChannel { Email, Sms, Push }

    public class NotificationRequest
    {
        public string Recipient { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public NotificationChannel Channel { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
        public int Priority { get; set; } = 5;
    }

    // PERF: struct without IEquatable<T> — boxing occurs when used in generic collections,
    //       .Equals does reflection-based comparison, defeats JIT optimizations
    public struct DeliveryResult
    {
        public string Recipient { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime SentAt { get; set; }
        public TimeSpan Duration { get; set; }

        public override string ToString()
        {
            return $"[{SentAt:HH:mm:ss}] {Recipient}: {(Success ? "OK" : "FAILED")} ({Duration.TotalMilliseconds:F0}ms)";
        }

        // PERF: Equals boxes 'obj' parameter and uses type check — not implementing IEquatable<DeliveryResult>
        public override bool Equals(object? obj)
        {
            if (obj is DeliveryResult other)
                return Recipient == other.Recipient && Success == other.Success && SentAt == other.SentAt;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Recipient, Success, SentAt);
        }
    }

    private readonly int _maxRetries;
    private readonly TimeSpan _retryDelay;
    private readonly List<DeliveryResult> _deliveryLog = new();

    public NotificationService(int maxRetries = 3, TimeSpan? retryDelay = null)
    {
        _maxRetries = maxRetries;
        _retryDelay = retryDelay ?? TimeSpan.FromSeconds(1);
    }

    public async Task<DeliveryResult> SendAsync(NotificationRequest request)
    {
        var startTime = DateTime.UtcNow;

        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            try
            {
                await DeliverAsync(request);

                var result = new DeliveryResult
                {
                    Recipient = request.Recipient,
                    Success = true,
                    SentAt = DateTime.UtcNow,
                    Duration = DateTime.UtcNow - startTime
                };

                // PERF: boxing — adding struct to List<DeliveryResult> is fine, but Equals uses boxing
                _deliveryLog.Add(result);
                return result;
            }
            catch (Exception ex)
            {
                if (attempt == _maxRetries)
                {
                    var result = new DeliveryResult
                    {
                        Recipient = request.Recipient,
                        Success = false,
                        ErrorMessage = ex.Message,
                        SentAt = DateTime.UtcNow,
                        Duration = DateTime.UtcNow - startTime
                    };

                    _deliveryLog.Add(result);
                    return result;
                }

                // PERF: Task.Delay in retry loop — no exponential back-off, no cancellation token
                await Task.Delay(_retryDelay);
            }
        }

        throw new InvalidOperationException("Unreachable");
    }

    public async Task<List<DeliveryResult>> SendBatchAsync(IEnumerable<NotificationRequest> requests)
    {
        var results = new List<DeliveryResult>();

        // PERF: .ToList() materialises the entire input just to sort
        var sorted = requests.OrderBy(r => r.Priority).ToList();

        // PERF: sequential async in a loop — no parallelism, each await blocks
        foreach (var request in sorted)
        {
            var result = await SendAsync(request);
            results.Add(result);
        }

        return results;
    }

    public async Task<List<DeliveryResult>> SendBatchParallelAsync(IEnumerable<NotificationRequest> requests)
    {
        var tasks = new List<Task<DeliveryResult>>();

        foreach (var request in requests)
        {
            // PERF: unbounded parallelism — fires all tasks at once, no throttling
            tasks.Add(SendAsync(request));
        }

        var results = await Task.WhenAll(tasks.ToArray());
        // PERF: .ToList() allocates a copy of the array
        return results.ToList();
    }

    private async Task DeliverAsync(NotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Recipient))
            throw new ArgumentException("Recipient is required");

        switch (request.Channel)
        {
            case NotificationChannel.Email:
                await SendEmailAsync(request);
                break;
            case NotificationChannel.Sms:
                await SendSmsAsync(request);
                break;
            case NotificationChannel.Push:
                await SendPushAsync(request);
                break;
        }
    }

    private async Task SendEmailAsync(NotificationRequest request)
    {
        // PERF: new HttpClient per call — socket exhaustion, should use IHttpClientFactory
        using var client = new HttpClient();
        // PERF: new JsonSerializerOptions per call
        var payload = JsonSerializer.Serialize(new
        {
            to = request.Recipient,
            subject = request.Subject,
            body = request.Body,
            metadata = request.Metadata
        });

        await Task.Delay(50); // simulate
    }

    private async Task SendSmsAsync(NotificationRequest request)
    {
        // PERF: new HttpClient per call
        using var client = new HttpClient();
        var message = request.Body;
        // PERF: .Substring allocates — on .NET 8+ could use Span/Range
        if (message.Length > 160)
            message = message.Substring(0, 157) + "...";

        await Task.Delay(30);
    }

    private async Task SendPushAsync(NotificationRequest request)
    {
        // PERF: new HttpClient per call
        using var client = new HttpClient();
        await Task.Delay(20);
    }

    // PERF: params array — allocates an array on every call
    public string FormatDeliveryReport(params DeliveryResult[] results)
    {
        // PERF: string concatenation instead of StringBuilder
        var report = "Delivery Report\n";
        report += "===============\n";

        // PERF: .Where().ToList() — two allocations (iterator + list) just to get a count
        var successful = results.Where(r => r.Success).ToList();
        var failed = results.Where(r => !r.Success).ToList();

        report += "Total: " + results.Length + "\n";
        report += "Successful: " + successful.Count + "\n";
        report += "Failed: " + failed.Count + "\n\n";

        if (failed.Count > 0)
        {
            report += "Failed deliveries:\n";
            foreach (var f in failed)
            {
                report += "  - " + f.Recipient + ": " + f.ErrorMessage + "\n";
            }
        }

        return report;
    }

    public Dictionary<string, object> GetStatistics()
    {
        var stats = new Dictionary<string, object>();
        // PERF: boxing int/double into Dictionary<string, object>
        stats["totalSent"] = _deliveryLog.Count;
        stats["successRate"] = _deliveryLog.Count > 0
            ? (double)_deliveryLog.Count(r => r.Success) / _deliveryLog.Count * 100
            : 0;
        stats["avgDurationMs"] = _deliveryLog.Count > 0
            ? _deliveryLog.Average(r => r.Duration.TotalMilliseconds)
            : 0;

        return stats;
    }
}
