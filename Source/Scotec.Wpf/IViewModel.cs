using System.ComponentModel;
using System.Windows.Controls;

namespace Scotec.Wpf;

/// <summary>
/// Defines the contract for a view model in the Scotec WPF framework.
/// </summary>
/// <remarks>
/// This interface extends <see cref="INotifyPropertyChanged" /> and <see cref="INotifyPropertyChanging" /> 
/// to provide property change notifications. It also includes support for data template selection 
/// through the <see cref="DataTemplateSelector" /> property.
/// </remarks>
public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChanging
{
    /// <summary>
    /// Gets the <see cref="DataTemplateSelector"/> used to dynamically select a data template
    /// for displaying the content of the family source settings view.
    /// </summary>
    /// <remarks>
    /// This property allows the view to determine the appropriate data template for rendering
    /// the content based on the data context. It is typically used in conjunction with a 
    /// <see cref="ContentPresenter"/> or similar controls in XAML.
    /// </remarks>

    public DataTemplateSelector? DataTemplateSelector { get; set; }
}
