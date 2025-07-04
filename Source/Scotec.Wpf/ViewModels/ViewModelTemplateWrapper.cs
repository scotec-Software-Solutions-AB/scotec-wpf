﻿using System;
using System.Windows;

namespace Scotec.Wpf.ViewModels;

public class ViewModelTemplateWrapper : DataTemplate
{
    public ViewModelTemplateWrapper(Type view)
    {
        if (view == null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        VisualTree = new FrameworkElementFactory(view);
    }
}
