using System.Text.Json;

namespace Perf01.Data;

/// <summary>
/// Transforms and queries JSON documents. Provides flattening, path-based access,
/// merging, and diff operations.
/// </summary>
public class JsonTransformer
{
    public Dictionary<string, object?> Flatten(string json)
    {
        // PERF: deserializes the entire document just to walk it — could use Utf8JsonReader
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        var result = new Dictionary<string, object?>();
        FlattenElement(doc, "", result);
        return result;
    }

    private void FlattenElement(JsonElement element, string prefix, Dictionary<string, object?> result)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in element.EnumerateObject())
                {
                    // PERF: string concatenation building keys — allocates on every level
                    var key = string.IsNullOrEmpty(prefix) ? prop.Name : prefix + "." + prop.Name;
                    FlattenElement(prop.Value, key, result);
                }
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var item in element.EnumerateArray())
                {
                    FlattenElement(item, prefix + "[" + index + "]", result);
                    index++;
                }
                break;

            default:
                // PERF: boxing for numeric/bool values into object?
                result[prefix] = element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.Number => element.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    _ => null
                };
                break;
        }
    }

    public string Merge(params string[] jsonDocuments)
    {
        var merged = new Dictionary<string, JsonElement>();

        foreach (var json in jsonDocuments)
        {
            // PERF: full deserialization of each document
            var doc = JsonSerializer.Deserialize<JsonElement>(json);
            if (doc.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in doc.EnumerateObject())
                {
                    merged[prop.Name] = prop.Value;
                }
            }
        }

        // PERF: new JsonSerializerOptions on every call — should be cached/static
        return JsonSerializer.Serialize(merged, new JsonSerializerOptions { WriteIndented = true });
    }

    public List<string> Diff(string json1, string json2)
    {
        // PERF: calls Flatten twice — each one deserializes + walks the full document
        var flat1 = Flatten(json1);
        var flat2 = Flatten(json2);
        var differences = new List<string>();

        // PERF: .ToList() + .Contains() for key lookups — O(n) per lookup instead of HashSet O(1)
        var allKeys = flat1.Keys.ToList();
        foreach (var key in flat2.Keys)
        {
            if (!allKeys.Contains(key))
                allKeys.Add(key);
        }

        foreach (var key in allKeys)
        {
            var in1 = flat1.ContainsKey(key);
            var in2 = flat2.ContainsKey(key);

            // PERF: string concatenation for building diff lines
            if (in1 && !in2)
                differences.Add("REMOVED: " + key + " = " + flat1[key]);
            else if (!in1 && in2)
                differences.Add("ADDED: " + key + " = " + flat2[key]);
            else if (in1 && in2 && !Equals(flat1[key], flat2[key]))
                differences.Add("CHANGED: " + key + " from " + flat1[key] + " to " + flat2[key]);
        }

        return differences;
    }

    public string Query(string json, string path)
    {
        var flat = Flatten(json);
        // PERF: .ToLower() without ordinal, called on every key
        var matches = flat
            .Where(kvp => kvp.Key.ToLower().Contains(path.ToLower()))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true });
    }

    public string Transform(string json, Dictionary<string, string> fieldMapping)
    {
        // PERF: flattens entire doc just to pick specific fields
        var source = Flatten(json);
        var result = new Dictionary<string, object?>();

        foreach (var mapping in fieldMapping)
        {
            var sourceKey = mapping.Value;
            var targetKey = mapping.Key;

            if (source.ContainsKey(sourceKey))
                result[targetKey] = source[sourceKey];
        }

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    public string PrettyPrint(string json)
    {
        var element = JsonSerializer.Deserialize<JsonElement>(json);
        // PERF: new JsonSerializerOptions on every call
        return JsonSerializer.Serialize(element, new JsonSerializerOptions { WriteIndented = true });
    }

    public List<Dictionary<string, object?>> FlattenArray(string jsonArray)
    {
        var array = JsonSerializer.Deserialize<JsonElement>(jsonArray);
        var results = new List<Dictionary<string, object?>>();

        if (array.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in array.EnumerateArray())
            {
                var flat = new Dictionary<string, object?>();
                FlattenElement(item, "", flat);
                results.Add(flat);
            }
        }

        return results;
    }
}
