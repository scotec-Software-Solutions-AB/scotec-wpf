using System;

namespace Scotec.Wpf.ViewModels;

/// <summary>
///     The view-model descriptor contains a mapping from a view-model to a associated view.
///     Typically, a view-model descriptor will be used in the applicatin configuration (e.g. autofac).
/// </summary>
public interface IViewModelDescriptor
{
    /// <summary>
    ///     Gets the type of the view-model.
    /// </summary>
    Type ViewModelType { get; }

    /// <summary>
    ///     Gets the type of the associated view.
    /// </summary>
    Type ViewType { get; }
}
