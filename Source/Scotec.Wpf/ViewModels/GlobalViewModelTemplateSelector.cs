#region

using System.Collections.Generic;
using System.Windows.Controls;

#endregion

namespace Scotec.Wpf.ViewModels;

public sealed class GlobalViewModelTemplateSelector : ViewModelTemplateSelector
{
    public GlobalViewModelTemplateSelector(IEnumerable<IViewModelDescriptor> viewModelDescriptors)
        : base(viewModelDescriptors, null)
    {
        DataTemplateSelector = this;
    }

    public static DataTemplateSelector? DataTemplateSelector { get; private set; }
}
