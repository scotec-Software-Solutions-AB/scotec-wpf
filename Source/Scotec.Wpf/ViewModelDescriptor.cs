using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Scotec.Wpf;

public class ViewModelDescriptor<TViewModel, TView> : IViewModelDescriptor where TViewModel : ObservableObject
{
    #region IViewModelDescriptor Members

    public Type ViewModelType => typeof(TViewModel);

    public Type ViewType => typeof(TView);

    #endregion
}
