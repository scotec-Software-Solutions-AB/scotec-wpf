using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Scotec.Wpf
{

    public class ViewModel : ObservableObject
    {
        private readonly ViewModelTemplateSelector? _templateSelector;

        public DataTemplateSelector? DataTemplateSelector => _templateSelector;

    }
}
