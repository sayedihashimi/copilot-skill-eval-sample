using System.Text.RegularExpressions;

namespace Perf01.TextProcessing;

/// <summary>
/// Generates URL-friendly slugs from input text.
/// </summary>
public class SlugGenerator
{
    // PERF: Static mutable dictionary — no concurrent safety, but also not FrozenDictionary on .NET 8+
    private static readonly Dictionary<string, string> ReplacementMap = new()
    {
        ["&"] = "and",
        ["+"] = "plus",
        ["@"] = "at",
        ["%"] = "percent",
        ["#"] = "hash",
        ["$"] = "dollar",
        ["€"] = "euro",
        ["£"] = "pound",
        ["¥"] = "yen",
    };

    public string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // PERF: .ToLower() without culture — Turkish-I problem + allocates
        var slug = input.ToLower();

        // PERF: iterating a dictionary to do sequential Replace — each Replace allocates a new string
        foreach (var kvp in ReplacementMap)
        {
            slug = slug.Replace(kvp.Key, kvp.Value);
        }

        // PERF: per-call static Regex.Replace — recompiles the regex every time
        slug = Regex.Replace(slug, @"[àáâãäå]", "a");
        slug = Regex.Replace(slug, @"[èéêë]", "e");
        slug = Regex.Replace(slug, @"[ìíîï]", "i");
        slug = Regex.Replace(slug, @"[òóôõö]", "o");
        slug = Regex.Replace(slug, @"[ùúûü]", "u");
        slug = Regex.Replace(slug, @"[ñ]", "n");
        slug = Regex.Replace(slug, @"[ç]", "c");
        slug = Regex.Replace(slug, @"[ß]", "ss");
        slug = Regex.Replace(slug, @"[æ]", "ae");
        slug = Regex.Replace(slug, @"[œ]", "oe");

        // PERF: more per-call static Regex.Replace calls
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"[\s-]+", "-");

        slug = slug.Trim('-');

        return slug;
    }

    public List<string> GenerateSlugs(IEnumerable<string> inputs)
    {
        // PERF: List without capacity hint
        var results = new List<string>();
        foreach (var input in inputs)
        {
            results.Add(GenerateSlug(input));
        }
        return results;
    }

    public string GenerateUniqueSlug(string input, IEnumerable<string> existingSlugs)
    {
        var baseSlug = GenerateSlug(input);

        // PERF: materialises the entire enumerable into a list, then uses .Contains (O(n)) in a loop
        var existing = existingSlugs.ToList();
        if (!existing.Contains(baseSlug))
            return baseSlug;

        var counter = 1;
        // PERF: repeated string concatenation + .ToString() inside a tight loop
        while (existing.Contains(baseSlug + "-" + counter.ToString()))
        {
            counter++;
        }
        return baseSlug + "-" + counter.ToString();
    }
}
