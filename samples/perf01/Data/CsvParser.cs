using System.Text.RegularExpressions;

namespace Perf01.Data;

/// <summary>
/// Parses CSV files into structured data. Handles quoted fields, escaped quotes, and multi-line values.
/// </summary>
public class CsvParser
{
    private readonly char _delimiter;
    private readonly char _quote;

    public CsvParser(char delimiter = ',', char quote = '"')
    {
        _delimiter = delimiter;
        _quote = quote;
    }

    public List<Dictionary<string, string>> Parse(string csvContent)
    {
        var lines = SplitLines(csvContent);
        if (lines.Count == 0)
            return new List<Dictionary<string, string>>();

        var headers = ParseLine(lines[0]);
        // PERF: List without capacity hint — we know the count is lines.Count - 1
        var results = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Count; i++)
        {
            var fields = ParseLine(lines[i]);
            var row = new Dictionary<string, string>();

            for (int j = 0; j < headers.Count; j++)
            {
                var value = j < fields.Count ? fields[j] : "";
                // PERF: .ToLower() without ordinal, allocates on every header key
                row[headers[j].Trim().ToLower()] = value.Trim();
            }

            results.Add(row);
        }

        return results;
    }

    private List<string> ParseLine(string line)
    {
        var fields = new List<string>();
        // PERF: string concatenation character-by-character — should use StringBuilder or Span
        var current = "";

        var inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == _quote)
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == _quote)
                {
                    current += _quote;
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (line[i] == _delimiter && !inQuotes)
            {
                fields.Add(current);
                current = "";
            }
            else
            {
                // PERF: += on string with a char — allocates new string each time
                current += line[i];
            }
        }

        fields.Add(current);
        return fields;
    }

    private List<string> SplitLines(string content)
    {
        var lines = new List<string>();
        // PERF: same char-by-char string concatenation pattern
        var currentLine = "";
        var inQuotes = false;

        for (int i = 0; i < content.Length; i++)
        {
            if (content[i] == _quote)
                inQuotes = !inQuotes;

            if (!inQuotes && (content[i] == '\n' || (content[i] == '\r' && i + 1 < content.Length && content[i + 1] == '\n')))
            {
                if (currentLine.Trim().Length > 0)
                    lines.Add(currentLine);
                currentLine = "";
                if (content[i] == '\r') i++;
            }
            else
            {
                currentLine += content[i];
            }
        }

        if (currentLine.Trim().Length > 0)
            lines.Add(currentLine);

        return lines;
    }

    public string Filter(string csvContent, string columnName, string pattern)
    {
        // PERF: re-parses the entire CSV just to filter, then re-formats
        var rows = Parse(csvContent);
        // PERF: new Regex per call — should cache or use static/compiled
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        // PERF: .ToLower() without ordinal
        var filtered = rows.Where(r => r.ContainsKey(columnName.ToLower()) && regex.IsMatch(r[columnName.ToLower()])).ToList();
        return FormatCsv(filtered);
    }

    public string Sort(string csvContent, string columnName, bool ascending = true)
    {
        var rows = Parse(csvContent);
        // PERF: .ToLower() inside LINQ lambda — called for every comparison
        var sorted = ascending
            ? rows.OrderBy(r => r.GetValueOrDefault(columnName.ToLower(), "")).ToList()
            : rows.OrderByDescending(r => r.GetValueOrDefault(columnName.ToLower(), "")).ToList();
        return FormatCsv(sorted);
    }

    public Dictionary<string, List<Dictionary<string, string>>> GroupBy(string csvContent, string columnName)
    {
        var rows = Parse(csvContent);
        var groups = new Dictionary<string, List<Dictionary<string, string>>>();

        foreach (var row in rows)
        {
            // PERF: .ToLower() per iteration
            var key = row.GetValueOrDefault(columnName.ToLower(), "(empty)");
            if (!groups.ContainsKey(key))
                groups[key] = new List<Dictionary<string, string>>();
            groups[key].Add(row);
        }

        return groups;
    }

    private string FormatCsv(List<Dictionary<string, string>> rows)
    {
        if (rows.Count == 0)
            return "";

        var headers = rows[0].Keys.ToList();
        // PERF: string concatenation for building CSV output — should use StringBuilder
        var sb = "";
        sb += string.Join(_delimiter.ToString(), headers) + "\n";

        foreach (var row in rows)
        {
            var values = new List<string>();
            foreach (var header in headers)
            {
                var value = row.GetValueOrDefault(header, "");
                // PERF: .Contains + .Replace inside nested loop — repeated allocations
                if (value.Contains(_delimiter) || value.Contains(_quote) || value.Contains('\n'))
                    value = _quote + value.Replace(_quote.ToString(), _quote.ToString() + _quote.ToString()) + _quote;
                values.Add(value);
            }
            sb += string.Join(_delimiter.ToString(), values) + "\n";
        }

        return sb;
    }
}
