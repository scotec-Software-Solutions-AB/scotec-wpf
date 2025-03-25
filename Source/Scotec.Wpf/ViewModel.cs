using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Scotec.Wpf;

public class ViewModel : ObservableObject
{
    private DataTemplateSelector? _dataTemplateSelector;

    public DataTemplateSelector? DataTemplateSelector
    {
        get => _dataTemplateSelector ?? GlobalViewModelTemplateSelector.DataTemplateSelector;
        set => _dataTemplateSelector = value;
    }
}
