namespace Scotec.Wpf.TextAudit;

public class UInt64TextAudit : TextAuditBase
{
    /// <inheritdoc />
    public override bool TestCombinedText(string text)
    {
        return IsValid(text);
    }

    /// <inheritdoc />
    public override bool IsValid(string text)
    {
        return ulong.TryParse(text, out _);
    }
}
