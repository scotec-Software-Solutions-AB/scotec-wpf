namespace Scotec.Wpf.TextAudit;

public class UInt32TextAudit : TextAuditBase
{
    /// <inheritdoc />
    public override bool TestCombinedText(string text)
    {
        return IsValid(text);
    }

    /// <inheritdoc />
    public override bool IsValid(string text)
    {
        return uint.TryParse(text, out _);
    }
}
