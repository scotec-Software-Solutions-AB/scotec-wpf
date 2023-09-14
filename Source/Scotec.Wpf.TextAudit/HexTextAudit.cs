using System.Globalization;

namespace Scotec.Wpf.TextAudit;

public class HexTextAudit : TextAuditBase
{
    public bool IsLowerCase { get; set; }
    //private static readonly Regex ValidCharacters = new ("[0-9a-fA-F]*"); 
    //public override bool TestInput(string input)
    //{
    //    return ValidCharacters.IsMatch(input);
    //}

    public override string PrepareInput(string input)
    {
        return IsLowerCase ? input.ToLower() : input.ToUpper();
    }

    public override bool TestCombinedText(string text)
    {
        return IsValid(text);
    }

    public override bool IsValid(string text)
    {
        return uint.TryParse(text, NumberStyles.AllowHexSpecifier, null, out var _);
    }
}
