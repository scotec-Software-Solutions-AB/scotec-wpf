using System.Windows.Input;

namespace Scotec.Wpf.TextAudit;

public abstract class TextAuditBase
{
    /// <summary>
    ///     Check if the string to be added contains invalid characters. Returns true if the string can be added, otherwise
    ///     false.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual bool TestInput(string input)
    {
        return true;
    }

    /// <summary>
    ///     Prepares the input text. The base implementations returns the original input text.
    ///     However, an overloaded method can change the input text to any value.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual string PrepareInput(string input)
    {
        return input;
    }

    /// <summary>
    ///     Inserts the string to be added at the current cursor position. Selected characters are removed. In overloaded
    ///     methods, the text to be inserted can be modified.The output parameter "insertedCharacters" returns the number of
    ///     characters actually inserted.Only characters may be inserted.The string may not otherwise be modified.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="input"></param>
    /// <param name="selectionStart"></param>
    /// <param name="selectionLength"></param>
    /// <param name="insertedCharacters"></param>
    /// <returns></returns>
    public virtual string CombineText(string text, string input, int selectionStart, int selectionLength, out int insertedCharacters)
    {
        var begin = text.Substring(0, selectionStart);
        var end = text.Substring(selectionStart + selectionLength);

        insertedCharacters = input.Length;

        return begin + input + end;
    }

    /// <summary>
    ///     Must be overwritten in derived classes. The method checks whether the string contains only valid characters and if
    ///     the order of the characters is valid.
    ///     Example: The number 123.456E12 is to be entered. So far the user has entered 123.456 and now enters an "E".
    ///     This results in the string 123.456E. This is an invalid value, but the sequence itself is valid.
    ///     <see cref="TestCombinedText" /> should return true
    ///     in this case while the method <see cref="IsValid" /> returns false because the number itself is not valid.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public abstract bool TestCombinedText(string text);

    /// <summary>
    ///     Returns true if the text can be used for further processing, otherwise false. This can be used to check if a button
    ///     can be activated in the UI.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public abstract bool IsValid(string text);

    /// <summary>
    ///     This method can be used in overloaded classes to check the currently pressed key for validity. The method is only
    ///     called for input via the keyboard.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual bool IsValidKey(KeyEventArgs args)
    {
        return !args.Handled;
    }

    /// <summary>
    ///     This method allows overridden classes to revise the entire text as well as set the cursor position and select
    ///     characters.
    ///     One possible application is, for example, auto-completion. Characters can be appended to the text and selected so
    ///     that they are highlighted
    ///     and can be overwritten the next time they are entered.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="selectionStart"></param>
    /// <param name="selectionLength"></param>
    /// <returns></returns>
    public virtual string? BuildReplacement(string text, ref int selectionStart, ref int selectionLength)
    {
        return text;
    }
}
