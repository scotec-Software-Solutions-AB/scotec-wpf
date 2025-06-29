using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Scotec.Wpf.ViewModels;

/// <summary>
///     Represents the base class for all view models in the Scotec WPF framework.
/// </summary>
/// <remarks>
///     This abstract class provides a foundation for implementing view models in WPF applications.
///     It extends the functionality of <see cref="ObservableObject" /> and implements the <see cref="IViewModel" />
///     interface.
///     The class includes support for data template selection through the <see cref="DataTemplateSelector" /> property.
/// </remarks>
public abstract class ViewModel : ObservableObject, IViewModel
{
    private DataTemplateSelector? _dataTemplateSelector;

    /// <summary>
    ///     Gets or sets the <see cref="DataTemplateSelector" /> used to select a data template
    ///     for the view associated with this view model.
    /// </summary>
    /// <remarks>
    ///     If no specific <see cref="DataTemplateSelector" /> is set, the global template selector
    ///     (<see cref="GlobalViewModelTemplateSelector.DataTemplateSelector" />) is used by default.
    ///     This property is commonly utilized to dynamically determine the appropriate data template
    ///     for a view model in WPF applications.
    /// </remarks>
    public DataTemplateSelector? DataTemplateSelector
    {
        get => _dataTemplateSelector ?? GlobalViewModelTemplateSelector.DataTemplateSelector;
        set => _dataTemplateSelector = value;
    }
}
