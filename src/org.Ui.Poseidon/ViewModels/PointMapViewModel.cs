using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Models;
using org.Ui.Poseidon.Views;
using org.Utils;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class PointMapViewModel : ObservableObject
    {
        private List<EmsPoint> _originPoints;

        [ObservableProperty]
        private BulkObservableCollection<PointViewModel> _points;

        [ObservableProperty]
        private PointViewModel _selectedPoint;

        [ObservableProperty]
        private string _groupName;

        [ObservableProperty]
        private double _cacheLength;

        public object Identifier { get; private set; }

        public PointMapViewModel(string groupName, List<EmsPoint> points, object identifier)
        {
            CacheLength = 500;
            GroupName = groupName;
            _originPoints = points;
            Identifier = identifier;
            Points = new BulkObservableCollection<PointViewModel>();
        }


        public void SingleViewLoading()
        {
            var pages = _originPoints?.Count ?? 0 / 500;
            for (int page = 0; page < pages; page++)
            {
                Points
                    .AddRange(
                        _originPoints
                            .Skip(page * (int)500)
                            .Take((int)500)
                            .Select(s => new PointViewModel(s)));

            }

        }
    }
}
