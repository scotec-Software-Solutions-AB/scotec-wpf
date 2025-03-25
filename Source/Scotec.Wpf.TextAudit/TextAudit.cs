using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scotec.Wpf.TextAudit;

public class TextAudit : DependencyObject
{
    public static readonly DependencyProperty AuditProperty = DependencyProperty.RegisterAttached("Audit",
        typeof(TextAuditBase), typeof(TextAudit)
        , new PropertyMetadata(null, PropertyChangedCallback));

    public static readonly DependencyPropertyKey IsValidPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
        "IsValid", typeof(bool), typeof(TextAudit)
        , new PropertyMetadata(false));

    public static TextAuditBase GetAudit(DependencyObject d)
    {
        return (TextAuditBase)d.GetValue(AuditProperty);
    }

    public static void SetAudit(DependencyObject d, TextAuditBase value)
    {
        d.SetValue(AuditProperty, value);
    }

    public static bool GetIsValid(DependencyObject d)
    {
        return (bool)d.GetValue(IsValidPropertyKey.DependencyProperty);
    }

    public static void SetIsValid(DependencyObject d, bool value)
    {
        d.SetValue(IsValidPropertyKey, value);
    }

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox)
        {
            DataObject.AddPastingHandler(textBox, OnPaste);

            textBox.PreviewKeyDown += OnPreviewKeyDown;
            textBox.PreviewTextInput += OnPreviewTextInput;
            textBox.TextChanged += OnTextChanged;
        }
    }

    private static void OnPaste(object sender, DataObjectPastingEventArgs args)
    {
        if (sender is TextBox textBox)
        {
            string? input = null;

            if (args.SourceDataObject.GetDataPresent(DataFormats.Text, true))
            {
                input = args.SourceDataObject.GetData(DataFormats.Text) as string;
            }

            if (input == null || !HandleInput(textBox, input, args.IsDragDrop))
            {
                args.CancelCommand();
                args.Handled = true;
                textBox.Focus();
            }
        }
    }

    private static void OnTextChanged(object sender, TextChangedEventArgs args)
    {
        if (sender is TextBox textBox)
        {
            var audit = GetAudit(textBox);
            SetIsValid(textBox, audit.IsValid(textBox.Text));
        }
    }

    private static void OnPreviewTextInput(object sender, TextCompositionEventArgs args)
    {
        if (sender is TextBox textBox)
        {
            if (!HandleInput(textBox, args.Text, false))
            {
                args.Handled = true;
            }
        }
    }

    private static void OnPreviewKeyDown(object sender, KeyEventArgs args)
    {
        if (sender is TextBox textBox)
        {
            var audit = GetAudit(textBox);
            if (!audit.IsValidKey(args))
            {
                args.Handled = true;
            }
        }
    }

    private static bool HandleInput(TextBox textBox, string input, bool isDragDrop)
    {
        var audit = GetAudit(textBox);

        var selectionStart = textBox.SelectionStart;
        var selectionLength = textBox.SelectionLength;

        var preparedInput = audit.PrepareInput(input);
        var text = audit.CombineText(textBox.Text, preparedInput, selectionStart, selectionLength, out var insertedChars);

        if (isDragDrop)
        {
            selectionLength = insertedChars;
        }
        else
        {
            selectionStart += insertedChars;
            selectionLength = 0;
        }

        if (!audit.TestCombinedText(text))
        {
            return false;
        }

        if (!isDragDrop)
        {
            text = audit.BuildReplacement(text, ref selectionStart, ref selectionLength);
        }

        textBox.Text = text;
        textBox.SelectionStart = selectionStart;
        textBox.SelectionLength = selectionLength;

        return false;
    }
}
