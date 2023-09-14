namespace Scotec.Wpf.TextAudit;

public class Int32TextAudit : TextAuditBase
{
    /// <inheritdoc />
    public override bool TestCombinedText(string text)
    {
        return text == "+" || text == "-" || IsValid(text);
    }

    /// <inheritdoc />
    public override bool IsValid(string text)
    {
        return int.TryParse(text, out _);
    }
}
