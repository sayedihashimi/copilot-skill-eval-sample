namespace Perf01.TextProcessing;

/// <summary>
/// Formats numbers with SI/metric unit prefixes (k, M, G, T, etc.).
/// Used in a monitoring dashboard rendering thousands of data points per second.
/// </summary>
public class MetricFormatter
{
    public struct UnitPrefix
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double Factor { get; set; }

        // PERF: struct lacks IEquatable<UnitPrefix> — boxing on equality checks in collections
    }

    // All SI prefixes from yocto to yotta
    private static readonly List<UnitPrefix> Prefixes = new()
    {
        new() { Symbol = "Y", Name = "yotta", Factor = 1e24 },
        new() { Symbol = "Z", Name = "zetta", Factor = 1e21 },
        new() { Symbol = "E", Name = "exa",   Factor = 1e18 },
        new() { Symbol = "P", Name = "peta",  Factor = 1e15 },
        new() { Symbol = "T", Name = "tera",  Factor = 1e12 },
        new() { Symbol = "G", Name = "giga",  Factor = 1e9 },
        new() { Symbol = "M", Name = "mega",  Factor = 1e6 },
        new() { Symbol = "k", Name = "kilo",  Factor = 1e3 },
        new() { Symbol = "h", Name = "hecto", Factor = 1e2 },
        new() { Symbol = "da", Name = "deca", Factor = 1e1 },
        new() { Symbol = "d", Name = "deci",  Factor = 1e-1 },
        new() { Symbol = "c", Name = "centi", Factor = 1e-2 },
        new() { Symbol = "m", Name = "milli", Factor = 1e-3 },
        new() { Symbol = "μ", Name = "micro", Factor = 1e-6 },
        new() { Symbol = "n", Name = "nano",  Factor = 1e-9 },
        new() { Symbol = "p", Name = "pico",  Factor = 1e-12 },
    };

    public string FormatWithPrefix(double value, string unit)
    {
        foreach (var prefix in Prefixes)
        {
            if (Math.Abs(value) >= prefix.Factor)
            {
                var scaled = value / prefix.Factor;
                return $"{scaled:F2} {prefix.Symbol}{unit}";
            }
        }
        return $"{value:F2} {unit}";
    }

    /// <summary>
    /// Expands abbreviated unit prefix symbols in a display string to full names.
    /// E.g., "5.2 kB" → "5.2 kiloB"
    /// </summary>
    public string ExpandPrefixSymbols(string displayValue)
    {
        // PERF: .Aggregate() calling .Replace() over 16 prefixes — creates 16 intermediate string allocations
        // Each .Replace() scans the entire string and allocates a new copy even if no match is found
        return Prefixes.Aggregate(displayValue, (current, prefix) =>
        {
            // PERF: char.ToString() allocation per prefix lookup iteration
            // (for single-char symbols, this allocates a new string each time)
            var symbol = prefix.Symbol.Length == 1 ? prefix.Symbol[0].ToString() : prefix.Symbol;
            return current.Replace(symbol, prefix.Name);
        });
    }

    /// <summary>
    /// Formats a batch of values with the best-fit prefix.
    /// </summary>
    public List<string> FormatBatch(IEnumerable<(double Value, string Unit)> items)
    {
        // PERF: List without capacity hint
        var results = new List<string>();
        foreach (var (value, unit) in items)
        {
            results.Add(FormatWithPrefix(value, unit));
        }
        return results;
    }
}

// ---------------------------------------------------------------------------
// Ordinalizer hierarchy — converts numbers to ordinal strings (1st, 2nd, etc.)
// for different locales.
// ---------------------------------------------------------------------------

/// <summary>
/// Base class for converting numbers to ordinal representation.
/// Abstract — should NOT be sealed.
/// </summary>
public abstract class Ordinalizer
{
    public abstract string Convert(int number);
    public abstract string Convert(int number, GrammaticalGender gender);

    public string ConvertBatch(IEnumerable<int> numbers)
    {
        // PERF: string concatenation in loop
        var result = "";
        foreach (var n in numbers)
        {
            result += Convert(n) + ", ";
        }
        return result.TrimEnd(',', ' ');
    }
}

public enum GrammaticalGender
{
    Masculine,
    Feminine,
    Neuter
}

/// <summary>
/// Default ordinalizer with English-like suffix rules.
/// This is a BASE class — EnglishOrdinalizer and GermanOrdinalizer extend it.
/// Should NOT be sealed.
/// </summary>
// PERF: unsealed class (correct — this is a base class with subclasses)
public class DefaultOrdinalizer : Ordinalizer
{
    public override string Convert(int number)
    {
        return Convert(number, GrammaticalGender.Masculine);
    }

    public override string Convert(int number, GrammaticalGender gender)
    {
        var suffix = GetSuffix(number);
        return number.ToString() + suffix;
    }

    protected virtual string GetSuffix(int number)
    {
        var abs = Math.Abs(number);
        var lastTwoDigits = abs % 100;
        var lastDigit = abs % 10;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            return "th";

        return lastDigit switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
    }
}

/// <summary>
/// English ordinalizer. Leaf class — should be sealed for JIT devirtualization.
/// </summary>
// PERF: unsealed leaf class — should be sealed for JIT devirtualization
public class EnglishOrdinalizer : DefaultOrdinalizer
{
    // English uses the same rules as Default — this subclass exists for
    // locale registration and potential future customization
    protected override string GetSuffix(int number)
    {
        return base.GetSuffix(number);
    }
}

/// <summary>
/// German ordinalizer. Leaf class — should be sealed for JIT devirtualization.
/// </summary>
// PERF: unsealed leaf class — should be sealed for JIT devirtualization
public class GermanOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, GrammaticalGender gender)
    {
        // German ordinals always end with a period
        return number.ToString() + ".";
    }
}

/// <summary>
/// Spanish ordinalizer. Leaf class — should be sealed for JIT devirtualization.
/// </summary>
// PERF: unsealed leaf class — should be sealed for JIT devirtualization
public class SpanishOrdinalizer : Ordinalizer
{
    public override string Convert(int number)
    {
        return Convert(number, GrammaticalGender.Masculine);
    }

    public override string Convert(int number, GrammaticalGender gender)
    {
        var suffix = gender switch
        {
            GrammaticalGender.Feminine => "ª",
            _ => "º"
        };
        return number.ToString() + suffix;
    }
}

/// <summary>
/// Registry for looking up ordinalizers by locale.
/// </summary>
public class OrdinalRegistry
{
    // Correct: uses OrdinalIgnoreCase — positive finding
    private static readonly Dictionary<string, Ordinalizer> Registry = new(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = new EnglishOrdinalizer(),
        ["en-US"] = new EnglishOrdinalizer(),
        ["en-GB"] = new EnglishOrdinalizer(),
        ["de"] = new GermanOrdinalizer(),
        ["de-DE"] = new GermanOrdinalizer(),
        ["es"] = new SpanishOrdinalizer(),
        ["es-ES"] = new SpanishOrdinalizer(),
    };

    public Ordinalizer GetOrdinalizer(string locale)
    {
        // PERF: ContainsKey + indexer instead of TryGetValue
        if (Registry.ContainsKey(locale))
            return Registry[locale];

        // Fallback: try language-only code
        var langCode = locale.Contains('-') ? locale.Split('-')[0] : locale;
        if (Registry.ContainsKey(langCode))
            return Registry[langCode];

        return new DefaultOrdinalizer();
    }
}
