namespace Scotec.Wpf.TextAudit;

public class Int64TextAudit : TextAuditBase
{
    /// <inheritdoc />
    public override bool TestCombinedText(string text)
    {
        return text == "+" || text == "-" || IsValid(text);
    }

    /// <inheritdoc />
    public override bool IsValid(string text)
    {
        return long.TryParse(text, out _);
    }
}
