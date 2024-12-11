﻿using System;

namespace Scotec.Wpf;

public class ViewModelDescriptor<TViewModel, TView> : IViewModelDescriptor where TViewModel : ViewModel
{
    #region IViewModelDescriptor Members

    public Type ViewModelType => typeof(TViewModel);

    public Type ViewType => typeof(TView);

    #endregion
}
