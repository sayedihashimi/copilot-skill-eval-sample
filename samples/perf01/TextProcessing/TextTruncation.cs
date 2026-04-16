namespace Perf01.TextProcessing;

/// <summary>
/// Text truncation library with multiple truncation strategies.
/// Used in a CMS to truncate user-facing content (titles, excerpts, previews).
/// </summary>
public interface ITruncator
{
    string Truncate(string value, int maxLength, string ellipsis = "…");
}

// PERF: List<char>[] for symbol sets — heap-allocated lists where ReadOnlySpan<char> or string would suffice
public static class TruncationSymbols
{
    public static readonly List<char> WhitespaceChars = new() { ' ', '\t', '\n', '\r' };
    public static readonly List<char> PunctuationChars = new() { '.', ',', ';', ':', '!', '?' };
    public static readonly List<char> BracketChars = new() { '(', ')', '[', ']', '{', '}' };
    public static readonly List<char> QuoteChars = new() { '"', '\'', '\u201C', '\u201D', '\u2018', '\u2019' };
}

/// <summary>
/// Truncates to a fixed character length.
/// </summary>
public class FixedLengthTruncator : ITruncator
{
    public string Truncate(string value, int maxLength, string ellipsis = "…")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        // PERF: value[..n] allocates a substring, then .TrimEnd() allocates another string — double allocation
        var truncated = value[..maxLength].TrimEnd();

        // PERF: .Contains on List<char> (O(n)) instead of a HashSet or direct char check
        while (truncated.Length > 0 && TruncationSymbols.PunctuationChars.Contains(truncated[^1]))
        {
            truncated = truncated[..^1];
        }

        return truncated + ellipsis;
    }
}

/// <summary>
/// Truncates to a fixed number of visible characters (skipping whitespace runs).
/// Uses Span-based comparison — the correct approach.
/// </summary>
public class FixedCharacterTruncator : ITruncator
{
    public string Truncate(string value, int maxLength, string ellipsis = "…")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        // Correct: uses AsSpan to avoid substring allocation for comparison
        var span = value.AsSpan();
        int visibleCount = 0;
        int endIndex = 0;

        for (int i = 0; i < span.Length && visibleCount < maxLength; i++)
        {
            if (!char.IsWhiteSpace(span[i]) || (i > 0 && !char.IsWhiteSpace(span[i - 1])))
            {
                visibleCount++;
            }
            endIndex = i + 1;
        }

        return value[..endIndex] + ellipsis;
    }
}

/// <summary>
/// Truncates at word boundaries.
/// </summary>
public class WordBoundaryTruncator : ITruncator
{
    public string Truncate(string value, int maxLength, string ellipsis = "…")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        // PERF: Substring instead of AsSpan — inconsistent with FixedCharacterTruncator
        var candidate = value.Substring(0, maxLength);

        var lastSpace = candidate.LastIndexOf(' ');
        if (lastSpace > 0)
        {
            // PERF: another Substring allocation
            candidate = candidate.Substring(0, lastSpace);
        }

        // PERF: .TrimEnd with char[] — could use ReadOnlySpan<char> overload
        candidate = candidate.TrimEnd(TruncationSymbols.WhitespaceChars.ToArray());

        return candidate + ellipsis;
    }
}

/// <summary>
/// Pipeline that applies multiple truncation strategies in sequence.
/// </summary>
public static class TruncationPipeline
{
    // PERF: params array allocates on every call — no single-argument fast-path overload
    // The common case is a single truncator, but this always allocates a params array
    public static string Apply(string value, int maxLength, params ITruncator[] truncators)
    {
        var result = value;
        foreach (var truncator in truncators)
        {
            result = truncator.Truncate(result, maxLength);
        }
        return result;
    }

    public static List<string> ApplyBatch(IEnumerable<string> values, int maxLength, params ITruncator[] truncators)
    {
        // PERF: List without capacity hint
        var results = new List<string>();
        foreach (var value in values)
        {
            results.Add(Apply(value, maxLength, truncators));
        }
        return results;
    }
}
