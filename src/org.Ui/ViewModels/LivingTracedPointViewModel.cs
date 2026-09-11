using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace org.Ui.ViewModels
{
    public partial class LivingTracedPointViewModel : ObservableObject
    {
        public LivingTracedPoint Point { get; private set; }

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private Brush _background;

        public ScottPlot.Color Color { get; private set; }

        public string Display => $"{Point.Key}:{(int)Point.FunctionCode}_{Point.Address.ToString().PadLeft(4, '0')}";

        public LivingTracedPointViewModel(LivingTracedPoint point, ScottPlot.Color color)
        {
            Point = point;
            Color = color;
            Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}
