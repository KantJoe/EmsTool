using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace org.Ui.ViewModels
{
    public partial class FaultViewModel : ObservableObject
    {
        [ObservableProperty]
        private ushort _code;
        [ObservableProperty]
        private Brush _background;

        public FaultViewModel(ushort code)
        {
            SetCode(code);
        }

        public void SetCode(ushort code)
        {
            Code = code;
            Background = new SolidColorBrush(code == 0 ? Colors.Green : Colors.Red);
        }
    }
}
