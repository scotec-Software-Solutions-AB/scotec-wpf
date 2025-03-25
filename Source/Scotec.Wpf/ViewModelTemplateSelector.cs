#region

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Scotec.Extensions.Linq;

#endregion

namespace Scotec.Wpf;

public class ViewModelTemplateSelector : DataTemplateSelector
{
    private readonly GlobalViewModelTemplateSelector? _globalViewModelTemplateSelector;
    private readonly Dictionary<Type, Type> _registry = new();

    public ViewModelTemplateSelector(IEnumerable<IViewModelDescriptor> viewModelDescriptors,
                                               GlobalViewModelTemplateSelector? globalViewModelTemplateSelector)
    {
        _globalViewModelTemplateSelector = globalViewModelTemplateSelector;
        Register(viewModelDescriptors);
    }

    public void Register(IViewModelDescriptor descriptor)
    {
        // Use the indexer to assign a new descriptor. This allows to overwrite the current descriptor fpr the given type.
        _registry[descriptor.ViewModelType] = descriptor.ViewType;
    }

    public void Register(IEnumerable<IViewModelDescriptor> descriptors)
    {
        descriptors.ForAll(Register);
    }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item == null)
        {
            return null;
        }

        if (!_registry.TryGetValue(item.GetType(), out var view))
        {
            // item.GetType() returns the implementation type. 
            // If the type has been registered by its interface we need to handle this here.

            var interfaces = item.GetType().GetInterfaces();

            // Start the search with the last interface.
            for (var i = interfaces.Length - 1; i >= 0; --i)
            {
                if (_registry.TryGetValue(interfaces[i], out view))
                {
                    break;
                }
            }
        }

        return view != null ? new ViewModelTemplateWrapper(view) : _globalViewModelTemplateSelector?.SelectTemplate(item, container);
    }
}
