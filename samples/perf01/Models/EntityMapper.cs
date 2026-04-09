namespace Perf01.Models;

/// <summary>
/// Maps between different entity representations (DTOs, domain models, view models).
/// Supports automatic property mapping, custom converters, and batch operations.
/// </summary>
public class EntityMapper
{
    // PERF: static Dictionary — on .NET 8+ could be FrozenDictionary since it never changes
    private static readonly Dictionary<string, Func<object, object>> Converters = new()
    {
        ["string->int"] = v => int.Parse(v.ToString()!),
        ["string->double"] = v => double.Parse(v.ToString()!),
        ["string->bool"] = v => bool.Parse(v.ToString()!),
        ["string->datetime"] = v => DateTime.Parse(v.ToString()!),
        ["int->string"] = v => v.ToString()!,
        ["double->string"] = v => v.ToString()!,
        ["bool->string"] = v => v.ToString()!.ToLower(),
        ["datetime->string"] = v => ((DateTime)v).ToString("o"),
    };

    // PERF: unsealed class
    public class MappingConfig
    {
        public Dictionary<string, string> FieldMappings { get; set; } = new();
        public Dictionary<string, Func<object, object>> CustomConverters { get; set; } = new();
        public HashSet<string> IgnoredFields { get; set; } = new();
        public bool StrictMode { get; set; } = false;
    }

    public Dictionary<string, object?> Map(
        Dictionary<string, object?> source,
        MappingConfig config)
    {
        var result = new Dictionary<string, object?>();

        foreach (var field in source)
        {
            if (config.IgnoredFields.Contains(field.Key))
                continue;

            // PERF: ContainsKey + indexer instead of TryGetValue
            var targetKey = config.FieldMappings.ContainsKey(field.Key)
                ? config.FieldMappings[field.Key]
                : field.Key;

            var value = field.Value;

            if (config.CustomConverters.ContainsKey(field.Key))
            {
                value = config.CustomConverters[field.Key](value!);
            }

            result[targetKey] = value;
        }

        return result;
    }

    public List<Dictionary<string, object?>> MapBatch(
        IEnumerable<Dictionary<string, object?>> sources,
        MappingConfig config)
    {
        // PERF: no capacity hint on List
        var results = new List<Dictionary<string, object?>>();
        foreach (var source in sources)
        {
            results.Add(Map(source, config));
        }
        return results;
    }

    public TTarget MapTo<TTarget>(Dictionary<string, object?> source) where TTarget : new()
    {
        var target = new TTarget();
        // PERF: GetProperties() via reflection on every call — should be cached per type
        var properties = typeof(TTarget).GetProperties();

        foreach (var prop in properties)
        {
            var key = prop.Name;
            if (!source.ContainsKey(key))
            {
                // PERF: .ToLower() without ordinal + LINQ FirstOrDefault on every miss
                key = source.Keys.FirstOrDefault(k => k.ToLower() == prop.Name.ToLower()) ?? "";
            }

            if (!string.IsNullOrEmpty(key) && source.ContainsKey(key))
            {
                var value = source[key];
                if (value != null)
                {
                    // PERF: .ToLower() on type names for lookup key building
                    var sourceType = value.GetType().Name.ToLower();
                    var targetType = prop.PropertyType.Name.ToLower();
                    var converterKey = sourceType + "->" + targetType;

                    if (sourceType != targetType && Converters.ContainsKey(converterKey))
                        value = Converters[converterKey](value);

                    // PERF: reflection SetValue on every property
                    prop.SetValue(target, value);
                }
            }
        }

        return target;
    }

    public Dictionary<string, object?> MapFrom<TSource>(TSource source) where TSource : class
    {
        var result = new Dictionary<string, object?>();
        // PERF: GetProperties() reflection on every call
        var properties = typeof(TSource).GetProperties();

        foreach (var prop in properties)
        {
            // PERF: reflection GetValue on every property
            result[prop.Name] = prop.GetValue(source);
        }

        return result;
    }

    public List<Dictionary<string, object?>> MapFromBatch<TSource>(IEnumerable<TSource> sources) where TSource : class
    {
        var results = new List<Dictionary<string, object?>>();
        foreach (var source in sources)
        {
            results.Add(MapFrom(source));
        }
        return results;
    }
}
