#region

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Scotec.Extensions.Linq;

#endregion

namespace Scotec.Wpf;

public sealed class GlobalViewModelTemplateSelector : ViewModelTemplateSelector
{
    public GlobalViewModelTemplateSelector(IEnumerable<IViewModelDescriptor> viewModelDescriptors)
    : base(viewModelDescriptors, null)
    {
        GlobalDataTemplateSelector = this;
    }

    public static DataTemplateSelector? GlobalDataTemplateSelector { get; private set; } 
}
