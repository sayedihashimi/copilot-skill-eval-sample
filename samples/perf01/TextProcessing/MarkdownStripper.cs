using System.Text.RegularExpressions;

namespace Perf01.TextProcessing;

/// <summary>
/// Strips markdown formatting from text to produce plain text output.
/// </summary>
public class MarkdownStripper
{
    // PERF: 40+ RegexOptions.Compiled instances — each one JIT-compiles at startup,
    //       blowing the compiled-regex startup budget. On .NET 8+ these should use
    //       [GeneratedRegex] source generators or just be non-compiled.
    private static readonly Regex HeaderPattern = new(@"^#{1,6}\s+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex BoldPattern = new(@"\*\*(.+?)\*\*", RegexOptions.Compiled);
    private static readonly Regex ItalicPattern = new(@"\*(.+?)\*", RegexOptions.Compiled);
    private static readonly Regex StrikethroughPattern = new(@"~~(.+?)~~", RegexOptions.Compiled);
    private static readonly Regex InlineCodePattern = new(@"`(.+?)`", RegexOptions.Compiled);
    private static readonly Regex LinkPattern = new(@"\[(.+?)\]\(.+?\)", RegexOptions.Compiled);
    private static readonly Regex ImagePattern = new(@"!\[.*?\]\(.+?\)", RegexOptions.Compiled);
    private static readonly Regex BlockquotePattern = new(@"^\s*>\s?", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex UnorderedListPattern = new(@"^\s*[-*+]\s+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex OrderedListPattern = new(@"^\s*\d+\.\s+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex HorizontalRulePattern = new(@"^[-*_]{3,}\s*$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex CodeBlockPattern = new(@"```[\s\S]*?```", RegexOptions.Compiled);
    private static readonly Regex HtmlTagPattern = new(@"<[^>]+>", RegexOptions.Compiled);
    private static readonly Regex MultipleNewlinesPattern = new(@"\n{3,}", RegexOptions.Compiled);
    private static readonly Regex MultipleSpacesPattern = new(@" {2,}", RegexOptions.Compiled);
    private static readonly Regex TableSeparatorPattern = new(@"^\|?[-:\s|]+\|?\s*$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex TableCellPattern = new(@"\|", RegexOptions.Compiled);
    private static readonly Regex FootnoteRefPattern = new(@"\[\^.+?\]", RegexOptions.Compiled);
    private static readonly Regex FootnoteDefPattern = new(@"^\[\^.+?\]:\s*", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex EmojiPattern = new(@":[\w+-]+:", RegexOptions.Compiled);
    private static readonly Regex EscapedCharPattern = new(@"\\([\\`*_\{\}\[\]()#+\-.!])", RegexOptions.Compiled);
    private static readonly Regex TaskListPattern = new(@"^\s*[-*]\s+\[[ xX]\]\s+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex DefinitionListPattern = new(@"^\s*:\s+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex HighlightPattern = new(@"==(.+?)==", RegexOptions.Compiled);
    private static readonly Regex SubscriptPattern = new(@"~(.+?)~", RegexOptions.Compiled);
    private static readonly Regex SuperscriptPattern = new(@"\^(.+?)\^", RegexOptions.Compiled);
    private static readonly Regex AbbreviationPattern = new(@"^\*\[.+?\]:\s*.+$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex AdmonitionPattern = new(@"^!!!\s+\w+.*$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex MathInlinePattern = new(@"\$(.+?)\$", RegexOptions.Compiled);
    private static readonly Regex MathBlockPattern = new(@"\$\$[\s\S]*?\$\$", RegexOptions.Compiled);
    private static readonly Regex FrontMatterPattern = new(@"^---[\s\S]*?---\s*", RegexOptions.Compiled);
    private static readonly Regex TocPattern = new(@"\[TOC\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex WikiLinkPattern = new(@"\[\[(.+?)(?:\|.+?)?\]\]", RegexOptions.Compiled);
    private static readonly Regex KeyboardPattern = new(@"<kbd>(.+?)</kbd>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex DetailsSummaryPattern = new(@"<details>[\s\S]*?</details>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex CommentPattern = new(@"<!--[\s\S]*?-->", RegexOptions.Compiled);
    private static readonly Regex IndentedCodePattern = new(@"^(?:\t| {4})(.+)$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex AutolinkPattern = new(@"<(https?://[^>]+)>", RegexOptions.Compiled);
    private static readonly Regex ReferenceDefPattern = new(@"^\s*\[.+?\]:\s+.+$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex SmartQuotePattern = new(@"[\u201C\u201D]", RegexOptions.Compiled);
    private static readonly Regex EmDashPattern = new(@"\u2014|---", RegexOptions.Compiled);
    private static readonly Regex EnDashPattern = new(@"\u2013|--", RegexOptions.Compiled);
    private static readonly Regex EllipsisPattern = new(@"\.\.\.", RegexOptions.Compiled);
    private static readonly Regex NonBreakingSpacePattern = new(@"&nbsp;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex LineBreakPattern = new(@"\s{2,}$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex CheckboxCheckedPattern = new(@"\[x\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex CheckboxUncheckedPattern = new(@"\[ \]", RegexOptions.Compiled);

    public string StripMarkdown(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        // PERF: each Replace call allocates a new string — long chain of allocations
        var text = markdown;
        text = FrontMatterPattern.Replace(text, "");
        text = CodeBlockPattern.Replace(text, "");
        text = IndentedCodePattern.Replace(text, "$1");
        text = CommentPattern.Replace(text, "");
        text = DetailsSummaryPattern.Replace(text, "");
        text = KeyboardPattern.Replace(text, "$1");
        text = HtmlTagPattern.Replace(text, "");
        text = MathBlockPattern.Replace(text, "");
        text = MathInlinePattern.Replace(text, "$1");
        text = HeaderPattern.Replace(text, "");
        text = HorizontalRulePattern.Replace(text, "");
        text = BlockquotePattern.Replace(text, "");
        text = AdmonitionPattern.Replace(text, "");
        text = TocPattern.Replace(text, "");
        text = ImagePattern.Replace(text, "");
        text = BoldPattern.Replace(text, "$1");
        text = ItalicPattern.Replace(text, "$1");
        text = StrikethroughPattern.Replace(text, "$1");
        text = HighlightPattern.Replace(text, "$1");
        text = SubscriptPattern.Replace(text, "$1");
        text = SuperscriptPattern.Replace(text, "$1");
        text = InlineCodePattern.Replace(text, "$1");
        text = LinkPattern.Replace(text, "$1");
        text = WikiLinkPattern.Replace(text, "$1");
        text = AutolinkPattern.Replace(text, "$1");
        text = EscapedCharPattern.Replace(text, "$1");
        text = TaskListPattern.Replace(text, "");
        text = UnorderedListPattern.Replace(text, "");
        text = OrderedListPattern.Replace(text, "");
        text = DefinitionListPattern.Replace(text, "");
        text = TableSeparatorPattern.Replace(text, "");
        text = TableCellPattern.Replace(text, " ");
        text = FootnoteDefPattern.Replace(text, "");
        text = FootnoteRefPattern.Replace(text, "");
        text = ReferenceDefPattern.Replace(text, "");
        text = AbbreviationPattern.Replace(text, "");
        text = EmojiPattern.Replace(text, "");
        text = CheckboxCheckedPattern.Replace(text, "☑");
        text = CheckboxUncheckedPattern.Replace(text, "☐");
        text = SmartQuotePattern.Replace(text, "\"");
        text = EmDashPattern.Replace(text, "—");
        text = EnDashPattern.Replace(text, "–");
        text = EllipsisPattern.Replace(text, "…");
        text = NonBreakingSpacePattern.Replace(text, " ");
        text = LineBreakPattern.Replace(text, "");
        text = MultipleNewlinesPattern.Replace(text, "\n\n");
        text = MultipleSpacesPattern.Replace(text, " ");

        return text.Trim();
    }

    public Dictionary<string, string> StripBatch(Dictionary<string, string> documents)
    {
        // PERF: new Dictionary without capacity
        var results = new Dictionary<string, string>();
        foreach (var doc in documents)
        {
            results[doc.Key] = StripMarkdown(doc.Value);
        }
        return results;
    }
}
