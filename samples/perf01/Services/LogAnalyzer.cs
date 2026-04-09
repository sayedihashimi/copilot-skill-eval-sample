using System.Text.RegularExpressions;

namespace Perf01.Services;

/// <summary>
/// Analyzes structured and unstructured log files to extract patterns,
/// error summaries, and timing information.
/// </summary>
public class LogAnalyzer
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = "";
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";
        public Dictionary<string, string> Properties { get; set; } = new();
    }

    public class AnalysisResult
    {
        public int TotalEntries { get; set; }
        public Dictionary<string, int> EntriesByLevel { get; set; } = new();
        public Dictionary<string, int> EntriesBySource { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public Dictionary<string, double> AverageResponseTimes { get; set; } = new();
        public List<string> Anomalies { get; set; } = new();
    }

    public List<LogEntry> ParseLog(string logContent)
    {
        var entries = new List<LogEntry>();
        var lines = logContent.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var entry = TryParseLine(line);
            if (entry != null)
                entries.Add(entry);
        }

        return entries;
    }

    private LogEntry? TryParseLine(string line)
    {
        // PERF: new Regex on every line — this is called per log line (potentially millions of times)
        var structured = new Regex(@"\[(\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d{3})\]\s+\[(\w+)\]\s+\[(\w+)\]\s+(.+)");
        var match = structured.Match(line);

        if (match.Success)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Parse(match.Groups[1].Value),
                // PERF: .ToUpper() allocates — could use StringComparison or store as-is
                Level = match.Groups[2].Value.ToUpper(),
                Source = match.Groups[3].Value,
                Message = match.Groups[4].Value
            };

            // PERF: another new Regex per matching line
            var kvRegex = new Regex(@"(\w+)=(""[^""]*""|\S+)");
            foreach (Match kvMatch in kvRegex.Matches(match.Groups[4].Value))
            {
                entry.Properties[kvMatch.Groups[1].Value] = kvMatch.Groups[2].Value.Trim('"');
            }

            return entry;
        }

        // PERF: second new Regex per line if first didn't match
        var commonLog = new Regex(@"(\S+)\s+(\S+)\s+(\S+)\s+\[(.+?)\]\s+""(.+?)""\s+(\d+)\s+(\d+)");
        match = commonLog.Match(line);

        if (match.Success)
        {
            return new LogEntry
            {
                Timestamp = DateTime.TryParse(match.Groups[4].Value, out var dt) ? dt : DateTime.MinValue,
                Level = "INFO",
                Source = match.Groups[1].Value,
                Message = match.Groups[5].Value,
                Properties = new Dictionary<string, string>
                {
                    ["status"] = match.Groups[6].Value,
                    ["bytes"] = match.Groups[7].Value
                }
            };
        }

        return null;
    }

    public AnalysisResult Analyze(string logContent)
    {
        var entries = ParseLog(logContent);
        var result = new AnalysisResult { TotalEntries = entries.Count };

        // PERF: three separate iterations over the same list where one would suffice
        foreach (var entry in entries)
        {
            if (!result.EntriesByLevel.ContainsKey(entry.Level))
                result.EntriesByLevel[entry.Level] = 0;
            result.EntriesByLevel[entry.Level]++;
        }

        foreach (var entry in entries)
        {
            if (!result.EntriesBySource.ContainsKey(entry.Source))
                result.EntriesBySource[entry.Source] = 0;
            result.EntriesBySource[entry.Source]++;
        }

        foreach (var entry in entries)
        {
            if (entry.Level == "ERROR")
                result.Errors.Add(entry.Timestamp + " [" + entry.Source + "] " + entry.Message);
            else if (entry.Level == "WARN" || entry.Level == "WARNING")
                result.Warnings.Add(entry.Timestamp + " [" + entry.Source + "] " + entry.Message);
        }

        // Calculate average response times
        var responseTimes = new Dictionary<string, List<double>>();
        foreach (var entry in entries)
        {
            if (entry.Properties.ContainsKey("duration_ms"))
            {
                var source = entry.Source;
                if (!responseTimes.ContainsKey(source))
                    responseTimes[source] = new List<double>();

                if (double.TryParse(entry.Properties["duration_ms"], out var duration))
                    responseTimes[source].Add(duration);
            }
        }

        foreach (var kvp in responseTimes)
        {
            result.AverageResponseTimes[kvp.Key] = kvp.Value.Average();
        }

        DetectAnomalies(entries, result);
        return result;
    }

    private void DetectAnomalies(List<LogEntry> entries, AnalysisResult result)
    {
        // PERF: materialises a filtered list just to iterate it
        var errorEntries = entries.Where(e => e.Level == "ERROR").ToList();

        for (int i = 0; i < errorEntries.Count - 5; i++)
        {
            // PERF: Skip(i).Take(5).ToList() allocates a new list on each iteration
            var window = errorEntries.Skip(i).Take(5).ToList();
            var timeSpan = window.Last().Timestamp - window.First().Timestamp;
            if (timeSpan.TotalSeconds < 10)
            {
                // PERF: string concatenation
                result.Anomalies.Add("Error burst detected: 5 errors in " +
                    timeSpan.TotalSeconds.ToString("F1") + "s starting at " + window.First().Timestamp);
            }
        }

        // PERF: double.Parse inside .Select — throws on malformed data instead of using TryParse
        var entriesWithDuration = entries
            .Where(e => e.Properties.ContainsKey("duration_ms"))
            .Select(e => new { Entry = e, Duration = double.Parse(e.Properties["duration_ms"]) })
            .ToList();

        if (entriesWithDuration.Count > 10)
        {
            var avg = entriesWithDuration.Average(e => e.Duration);
            // PERF: iterates the list twice (once for avg, once for stddev), then a third time for spikes
            var stdDev = Math.Sqrt(entriesWithDuration.Average(e => Math.Pow(e.Duration - avg, 2)));

            foreach (var e in entriesWithDuration)
            {
                if (e.Duration > avg + 3 * stdDev)
                {
                    result.Anomalies.Add("Response time spike: " + e.Duration.ToString("F0") +
                        "ms (avg: " + avg.ToString("F0") + "ms) at " + e.Entry.Timestamp);
                }
            }
        }
    }

    public Dictionary<string, int> ExtractErrorCodes(string logContent)
    {
        // PERF: re-parses the entire log (already parsed once if Analyze was called)
        var entries = ParseLog(logContent);
        var errorCodes = new Dictionary<string, int>();

        // PERF: new Regex per call
        var codePattern = new Regex(@"(?:error|err|code)[:\s]+([A-Z]{2,5}\d{3,5})", RegexOptions.IgnoreCase);

        foreach (var entry in entries)
        {
            var matches = codePattern.Matches(entry.Message);
            foreach (Match match in matches)
            {
                // PERF: .ToUpper() allocates
                var code = match.Groups[1].Value.ToUpper();
                if (!errorCodes.ContainsKey(code))
                    errorCodes[code] = 0;
                errorCodes[code]++;
            }
        }

        return errorCodes;
    }

    public string Summarize(string logContent)
    {
        var result = Analyze(logContent);
        // PERF: massive string concatenation — should use StringBuilder
        var summary = "";

        summary += "=== Log Analysis Summary ===" + "\n";
        summary += "Total entries: " + result.TotalEntries + "\n\n";

        summary += "By Level:" + "\n";
        foreach (var level in result.EntriesByLevel.OrderByDescending(x => x.Value))
            summary += "  " + level.Key + ": " + level.Value + "\n";

        summary += "\nBy Source:" + "\n";
        foreach (var source in result.EntriesBySource.OrderByDescending(x => x.Value).Take(10))
            summary += "  " + source.Key + ": " + source.Value + "\n";

        if (result.Errors.Count > 0)
        {
            summary += "\nRecent Errors (last 5):" + "\n";
            foreach (var error in result.Errors.TakeLast(5))
                summary += "  " + error + "\n";
        }

        if (result.AverageResponseTimes.Count > 0)
        {
            summary += "\nAverage Response Times:" + "\n";
            foreach (var rt in result.AverageResponseTimes.OrderByDescending(x => x.Value))
                summary += "  " + rt.Key + ": " + rt.Value.ToString("F1") + "ms" + "\n";
        }

        if (result.Anomalies.Count > 0)
        {
            summary += "\nAnomalies:" + "\n";
            foreach (var anomaly in result.Anomalies)
                summary += "  ⚠ " + anomaly + "\n";
        }

        return summary;
    }
}
