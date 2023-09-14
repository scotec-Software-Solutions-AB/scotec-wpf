using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Scotec.Wpf.TextAudit;

public class DoubleTextAudit : TextAuditBase
{
    private readonly Regex _validSequence =
        new("^([+-]?[0-9]*(" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "[0-9]*)?([eE][+-]?[0-9]*)?)$");

    protected readonly string DecimalSeperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

    /// <inheritdoc />
    public override bool TestCombinedText(string text)
    {
        return _validSequence.IsMatch(text);

        //return text == "+" || text == "-"
        //                   || text.StartsWith("+") || text.StartsWith("-")
        //                   || text.EndsWith("+") || text.EndsWith("-")
        //                   || text == DecimalSeperator 
        //                   || "E".Equals(text, StringComparison.InvariantCultureIgnoreCase) 
        //                   || text.StartsWith("E", StringComparison.InvariantCultureIgnoreCase) 
        //                   || text.EndsWith("E", StringComparison.InvariantCultureIgnoreCase) 
        //                   || IsValid(text);
    }

    /// <inheritdoc />
    public override bool IsValid(string text)
    {
        return double.TryParse(text, out _);
    }

    /// <inheritdoc />
    public override string? BuildReplacement(string text, ref int selectionStart, ref int selectionLength)
    {
        var replacement = text;
        var beginsWithComma = text.StartsWith(DecimalSeperator);
        var endsWithComma = text.EndsWith(DecimalSeperator);

        var beginsWithE = text.StartsWith("E", StringComparison.InvariantCultureIgnoreCase);
        var endsWithE = text.EndsWith("E", StringComparison.InvariantCultureIgnoreCase);

        var beginsWithPlusOrMinus = text.StartsWith("+") || text.StartsWith("-");
        var endsWithPlusOrMinus = text.EndsWith("+") || text.EndsWith("-");
        var containsE = text.ToUpper().Contains("E");

        if (beginsWithComma || beginsWithE)
        {
            selectionStart += 1;
            selectionLength = 0;
            replacement = "0" + replacement;
        }

        if (endsWithComma || endsWithE)
        {
            selectionLength = 1;
            replacement += "0";
        }

        if (endsWithPlusOrMinus)
        {
            selectionLength = 1;
            replacement += "0";
        }

        return replacement;
    }
}
