using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scotec.Wpf.TextAudit;

public class AuditTextBox : TextBox
{
    public static readonly DependencyProperty TextAuditProperty =
        DependencyProperty.Register("TextAudit", typeof(TextAuditBase), typeof(TextBox));

    public static readonly DependencyPropertyKey IsValidPropertyKey =
        DependencyProperty.RegisterReadOnly("IsValid", typeof(bool), typeof(TextBox), new PropertyMetadata(false));

    public AuditTextBox()
    {
        DataObject.AddPastingHandler(this, OnPaste);
    }

    public TextAuditBase? TextAudit
    {
        get => (TextAuditBase)GetValue(TextAuditProperty);
        set => SetValue(TextAuditProperty, value);
    }

    public bool IsValid
    {
        get => (bool)GetValue(IsValidPropertyKey.DependencyProperty);
        private set => SetValue(IsValidPropertyKey, value);
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        IsValid = TextAudit?.IsValid(Text) ?? true;
    }

    protected override void OnPreviewKeyDown(KeyEventArgs args)
    {
        if (TextAudit != null && !TextAudit.IsValidKey(args))
        {
            args.Handled = true;
            return;
        }

        base.OnPreviewKeyDown(args);
    }

    protected override void OnPreviewTextInput(TextCompositionEventArgs args)
    {
        if (!HandleInput(args.Text))
        {
            args.Handled = true;
            return;
        }

        base.OnPreviewTextInput(args);
    }

    private void OnPaste(object sender, DataObjectPastingEventArgs args)
    {
        string? text = null;

        if (args.SourceDataObject.GetDataPresent(DataFormats.Text, true))
        {
            text = args.SourceDataObject.GetData(DataFormats.Text) as string;
        }

        if (text == null || !HandleInput(text))
        {
            args.CancelCommand();
            args.Handled = true;
        }
    }

    private bool HandleInput(string input)
    {
        if (TextAudit == null)
        {
            return true;
        }

        var selectionStart = SelectionStart;
        var selectionLength = SelectionLength;
        var text = TextAudit.CombineText(Text, input, selectionStart, selectionLength, out var insertedChars);

        selectionStart += insertedChars;
        selectionLength = 0;

        if (!TextAudit.TestCombinedText(text))
        {
            return false;
        }

        var replacement = TextAudit.BuildReplacement(text, ref selectionStart, ref selectionLength);
        if (replacement != null)
        {
            Text = replacement;
            if (selectionStart > -1 && selectionLength > -1)
            {
                SelectionStart = selectionStart;
                SelectionLength = selectionLength;
            }

            return false;
        }

        return true;
    }
}
