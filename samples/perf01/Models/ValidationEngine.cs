using System.Text.RegularExpressions;

namespace Perf01.Models;

/// <summary>
/// Validates data objects against configurable rules. Supports
/// field-level validation, cross-field rules, and custom validators.
/// </summary>
public class ValidationEngine
{
    // PERF: struct without IEquatable<ValidationError>
    public struct ValidationError
    {
        public string Field { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        // PERF: string interpolation allocates in ToString — called frequently for logging
        public override string ToString() => $"[{Code}] {Field}: {Message}";
    }

    // PERF: unsealed class
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<ValidationError> Errors { get; set; } = new();
    }

    private readonly List<(string Field, Func<string, bool> Rule, string Code, string Message)> _rules = new();

    public ValidationEngine AddRequired(string field)
    {
        // PERF: string concatenation for message — minor, but captured in closure
        _rules.Add((field, v => !string.IsNullOrWhiteSpace(v), "REQUIRED", field + " is required"));
        return this;
    }

    public ValidationEngine AddMinLength(string field, int length)
    {
        _rules.Add((field, v => v?.Length >= length, "MIN_LENGTH",
            field + " must be at least " + length + " characters"));
        return this;
    }

    public ValidationEngine AddMaxLength(string field, int length)
    {
        _rules.Add((field, v => v == null || v.Length <= length, "MAX_LENGTH",
            field + " must be at most " + length + " characters"));
        return this;
    }

    public ValidationEngine AddPattern(string field, string pattern, string? message = null)
    {
        _rules.Add((field, v =>
        {
            if (string.IsNullOrEmpty(v)) return true;
            // PERF: new Regex on every validation call — pattern is constant, should be compiled once
            return new Regex(pattern).IsMatch(v);
        }, "PATTERN", message ?? field + " does not match expected pattern"));
        return this;
    }

    public ValidationEngine AddEmail(string field)
    {
        return AddPattern(field, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            field + " must be a valid email");
    }

    public ValidationEngine AddUrl(string field)
    {
        return AddPattern(field, @"^https?://[^\s]+$", field + " must be a valid URL");
    }

    public ValidationEngine AddPhoneNumber(string field)
    {
        return AddPattern(field, @"^\+?[\d\s\-()]{7,15}$", field + " must be a valid phone number");
    }

    public ValidationEngine AddRange(string field, double min, double max)
    {
        _rules.Add((field, v =>
        {
            if (string.IsNullOrEmpty(v)) return true;
            return double.TryParse(v, out var num) && num >= min && num <= max;
        }, "RANGE", field + " must be between " + min + " and " + max));
        return this;
    }

    public ValidationEngine AddCustom(string field, Func<string, bool> rule, string code, string message)
    {
        _rules.Add((field, rule, code, message));
        return this;
    }

    public ValidationResult Validate(Dictionary<string, string> data)
    {
        var result = new ValidationResult();

        foreach (var (field, rule, code, message) in _rules)
        {
            // PERF: ContainsKey + indexer instead of TryGetValue
            var value = data.ContainsKey(field) ? data[field] : null;

            if (!rule(value!))
            {
                result.Errors.Add(new ValidationError
                {
                    Field = field,
                    Code = code,
                    Message = message
                });
            }
        }

        return result;
    }

    public List<ValidationResult> ValidateBatch(IEnumerable<Dictionary<string, string>> items)
    {
        // PERF: List without capacity hint
        var results = new List<ValidationResult>();
        foreach (var item in items)
        {
            results.Add(Validate(item));
        }
        return results;
    }

    public string FormatErrors(ValidationResult result)
    {
        if (result.IsValid)
            return "Validation passed.";

        // PERF: string concatenation for output building
        var output = "Validation failed with " + result.Errors.Count + " error(s):\n";
        foreach (var error in result.Errors)
        {
            output += "  ✗ " + error.ToString() + "\n";
        }
        return output;
    }

    public static ValidationEngine CreateUserValidator()
    {
        return new ValidationEngine()
            .AddRequired("name")
            .AddMinLength("name", 2)
            .AddMaxLength("name", 100)
            .AddRequired("email")
            .AddEmail("email")
            .AddPhoneNumber("phone")
            .AddRange("age", 0, 150)
            .AddUrl("website");
    }
}
