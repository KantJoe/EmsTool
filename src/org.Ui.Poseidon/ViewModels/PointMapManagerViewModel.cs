using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using org.Ui.Poseidon.Views;
using org.Ui.Views;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class PointMapManagerViewModel : ObservableObject
    {
        private bool _initialed;
        [ObservableProperty]
        private ObservableCollection<PointMapProxyViewModel> _items;

        [ObservableProperty]
        private PointMapProxyViewModel _selectedItem;

        public WeakReference<EmsSolution> Solution { get; private set; }

        public PointMapManagerViewModel()
        {
            Solution = new WeakReference<EmsSolution>(EmsSolutionContext.Current);
            //Solution.TryGetTarget(out EmsSolution solution);

            //var maps = solution?.DeviceTopologies;

            //var maps = Enumerable.Range(22, 10).Select(s => new EmsDevice { Alias = "COM" + s })
            //    .ToList();

            //maps[0] = solution.DeviceTopologies[0];
            //Items = [.. maps.Select(s => new PointMapProxyViewModel(s)).ToList()];
        }

        public void Initialize()
        {
            if (_initialed)
            {
                return;
            }

            Solution.TryGetTarget(out EmsSolution solution);
            var maps = solution?.DeviceTopologies;

            DialogHost.Show(new WaitDialog(), UiGlobalContext.RootDialog, new DialogOpenedEventHandler((s, e) =>
            {
                Items = [.. maps.Select(s => new PointMapProxyViewModel(s)).ToList()];
            }));

            //var maps = Enumerable.Range(22, 10).Select(s => new EmsDevice { Alias = "COM" + s })
            //    .ToList();

            //maps[0] = solution.DeviceTopologies[0];
            _initialed = true;
        }
    }
}
