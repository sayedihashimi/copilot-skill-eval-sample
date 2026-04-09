namespace Perf01.Services;

/// <summary>
/// Configurable data transformation pipeline that processes records through
/// a series of stages: validation, transformation, enrichment, and output.
/// </summary>
public class DataPipeline
{
    // PERF: unsealed class — JIT cannot devirtualize calls to Record
    public class Record
    {
        public string Id { get; set; } = "";
        public Dictionary<string, string> Fields { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public DateTime ProcessedAt { get; set; }
    }

    public class PipelineResult
    {
        public List<Record> Processed { get; set; } = new();
        public List<(Record Record, string Error)> Failed { get; set; } = new();
        public TimeSpan Duration { get; set; }
        public Dictionary<string, int> StageMetrics { get; set; } = new();
    }

    private readonly List<Func<Record, Record?>> _validators = new();
    private readonly List<Func<Record, Record>> _transformers = new();
    private readonly List<Func<Record, Record>> _enrichers = new();

    public DataPipeline AddValidator(Func<Record, Record?> validator)
    {
        _validators.Add(validator);
        return this;
    }

    public DataPipeline AddTransformer(Func<Record, Record> transformer)
    {
        _transformers.Add(transformer);
        return this;
    }

    public DataPipeline AddEnricher(Func<Record, Record> enricher)
    {
        _enrichers.Add(enricher);
        return this;
    }

    public PipelineResult Execute(IEnumerable<Record> records)
    {
        var startTime = DateTime.UtcNow;
        var result = new PipelineResult();

        foreach (var record in records)
        {
            try
            {
                var current = record;

                var valid = true;
                foreach (var validator in _validators)
                {
                    var validated = validator(current);
                    if (validated == null)
                    {
                        valid = false;
                        result.Failed.Add((record, "Validation failed"));
                        break;
                    }
                    current = validated;
                }

                if (!valid) continue;

                foreach (var transformer in _transformers)
                {
                    current = transformer(current);
                }

                foreach (var enricher in _enrichers)
                {
                    current = enricher(current);
                }

                current.ProcessedAt = DateTime.UtcNow;
                result.Processed.Add(current);
            }
            catch (Exception ex)
            {
                // PERF: exception-driven flow — catching generic Exception for expected failures
                result.Failed.Add((record, ex.Message));
            }
        }

        result.Duration = DateTime.UtcNow - startTime;

        result.StageMetrics["validators"] = _validators.Count;
        result.StageMetrics["transformers"] = _transformers.Count;
        result.StageMetrics["enrichers"] = _enrichers.Count;
        result.StageMetrics["processed"] = result.Processed.Count;
        result.StageMetrics["failed"] = result.Failed.Count;

        return result;
    }

    public string FormatReport(PipelineResult result)
    {
        // PERF: string concatenation for building report
        var report = "";
        report += "Pipeline Execution Report" + "\n";
        report += "========================" + "\n";
        report += "Duration: " + result.Duration.TotalMilliseconds.ToString("F0") + "ms" + "\n";
        report += "Processed: " + result.Processed.Count + "\n";
        report += "Failed: " + result.Failed.Count + "\n";

        if (result.Failed.Count > 0)
        {
            report += "\nFailures:\n";
            foreach (var failure in result.Failed)
            {
                report += "  " + failure.Record.Id + ": " + failure.Error + "\n";
            }
        }

        // PERF: nested loop with dictionary lookups — ContainsKey + indexer instead of TryGetValue
        var tagCounts = new Dictionary<string, int>();
        foreach (var record in result.Processed)
        {
            foreach (var tag in record.Tags)
            {
                if (!tagCounts.ContainsKey(tag))
                    tagCounts[tag] = 0;
                tagCounts[tag]++;
            }
        }

        if (tagCounts.Count > 0)
        {
            report += "\nTag Distribution:\n";
            foreach (var tag in tagCounts.OrderByDescending(t => t.Value))
            {
                report += "  " + tag.Key + ": " + tag.Value + "\n";
            }
        }

        return report;
    }

    public static DataPipeline CreateStandardPipeline()
    {
        return new DataPipeline()
            .AddValidator(r =>
            {
                if (string.IsNullOrEmpty(r.Id)) return null;
                if (r.Fields.Count == 0) return null;
                return r;
            })
            .AddTransformer(r =>
            {
                // PERF: new Dictionary per record + .ToLower() per field key
                var normalized = new Dictionary<string, string>();
                foreach (var field in r.Fields)
                {
                    normalized[field.Key.ToLower()] = field.Value.Trim();
                }
                r.Fields = normalized;
                return r;
            })
            .AddTransformer(r =>
            {
                var cleaned = new Dictionary<string, string>();
                foreach (var field in r.Fields)
                {
                    if (!string.IsNullOrWhiteSpace(field.Value))
                        cleaned[field.Key] = field.Value;
                }
                r.Fields = cleaned;
                return r;
            })
            .AddEnricher(r =>
            {
                foreach (var field in r.Fields)
                {
                    // PERF: .Contains on string called in nested loop
                    if (field.Value.Contains("@") && field.Value.Contains("."))
                        r.Tags.Add("has-email");
                    // PERF: .Any(char.IsDigit) — LINQ overhead on every field value
                    if (field.Value.Any(char.IsDigit))
                        r.Tags.Add("has-numbers");
                }
                // PERF: .Distinct().ToList() allocates two collections to deduplicate
                r.Tags = r.Tags.Distinct().ToList();
                return r;
            });
    }
}
