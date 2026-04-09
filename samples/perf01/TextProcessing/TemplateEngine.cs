using System.Text.RegularExpressions;

namespace Perf01.TextProcessing;

/// <summary>
/// Simple template engine that replaces placeholders with values.
/// Supports {{variable}}, {{#if condition}}...{{/if}}, and {{#each list}}...{{/each}}.
/// </summary>
public class TemplateEngine
{
    public string Render(string template, Dictionary<string, object> context)
    {
        if (string.IsNullOrEmpty(template))
            return string.Empty;

        var result = template;
        result = ProcessConditionals(result, context);
        result = ProcessLoops(result, context);
        result = ReplaceVariables(result, context);

        // PERF: per-call Regex — compiled on every invocation
        result = Regex.Replace(result, @"\{\{.+?\}\}", "");

        return result;
    }

    private string ProcessConditionals(string template, Dictionary<string, object> context)
    {
        // PERF: new Regex allocated every time this method is called
        var pattern = new Regex(@"\{\{#if\s+(\w+)\}\}([\s\S]*?)\{\{/if\}\}");
        return pattern.Replace(template, match =>
        {
            var key = match.Groups[1].Value;
            if (context.TryGetValue(key, out var value) && IsTruthy(value))
                return match.Groups[2].Value;
            return string.Empty;
        });
    }

    private string ProcessLoops(string template, Dictionary<string, object> context)
    {
        // PERF: new Regex allocated every call
        var pattern = new Regex(@"\{\{#each\s+(\w+)\}\}([\s\S]*?)\{\{/each\}\}");
        return pattern.Replace(template, match =>
        {
            var key = match.Groups[1].Value;
            var body = match.Groups[2].Value;

            if (!context.TryGetValue(key, out var value) || value is not IEnumerable<object> items)
                return string.Empty;

            // PERF: string concatenation in a loop — O(n²) allocation pattern
            var output = "";
            var index = 0;
            foreach (var item in items)
            {
                // PERF: new Dictionary per iteration, copying entire context
                var iterContext = new Dictionary<string, object>(context)
                {
                    ["item"] = item,
                    ["index"] = index
                };
                output += ReplaceVariables(body, iterContext);
                index++;
            }
            return output;
        });
    }

    private string ReplaceVariables(string template, Dictionary<string, object> context)
    {
        // PERF: per-call Regex.Replace with a new pattern
        return Regex.Replace(template, @"\{\{(\w+(?:\.\w+)*)\}\}", match =>
        {
            var path = match.Groups[1].Value;
            var value = ResolveValue(path, context);
            return value?.ToString() ?? "";
        });
    }

    private object? ResolveValue(string path, Dictionary<string, object> context)
    {
        var parts = path.Split('.');
        if (!context.TryGetValue(parts[0], out var root))
            return null;

        object? current = root;
        for (int i = 1; i < parts.Length; i++)
        {
            if (current is Dictionary<string, object> dict)
            {
                if (!dict.TryGetValue(parts[i], out var next))
                    return null;
                current = next;
            }
            else
            {
                // PERF: reflection on every property access in hot path
                var prop = current?.GetType().GetProperty(parts[i]);
                current = prop?.GetValue(current);
            }
        }

        return current;
    }

    private bool IsTruthy(object? value) => value switch
    {
        null => false,
        bool b => b,
        int n => n != 0,
        string s => !string.IsNullOrEmpty(s),
        // PERF: .Any() forces enumeration just to check non-empty
        IEnumerable<object> list => list.Any(),
        _ => true
    };

    public string RenderBatch(string template, IEnumerable<Dictionary<string, object>> contexts)
    {
        // PERF: string concatenation in a loop
        var output = "";
        foreach (var context in contexts)
        {
            output += Render(template, context) + "\n";
        }
        return output;
    }
}
