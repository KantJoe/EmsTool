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
using System.Diagnostics.Tracing;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class PointMapProxyViewModel : ObservableObject
    {
        private EmsPointMap _pointMap;

        [ObservableProperty]
        private PointMapViewModel _pointMaps;

        [ObservableProperty]
        private ObservableCollection<PointMapViewModel> _pointMapsOf4;

        [ObservableProperty]
        private PointMapViewModel _selectedPointMapOf4;


        [ObservableProperty]
        private ObservableCollection<PointMapViewModel> _pointMapsOf3;

        [ObservableProperty]
        private PointMapViewModel _selectedPointMapOf3;

        [ObservableProperty]
        private bool _multipleGroups = true;

        [ObservableProperty]
        private object _content;

        [ObservableProperty]
        private object _identifier;

        [ObservableProperty]
        private double _cacheLength;


        public int OrderNumber
        {
            get => Device?.OrderNumber ?? 0;
            set
            {
                var v = Device?.OrderNumber ?? 0;
                if (Device is not null && SetProperty(ref v, value, nameof(OrderNumber)))
                {
                    Device.OrderNumber = v;
                }
            }
        }

        public CommunicateType CommunicateType
        {
            get => Device?.CommunicateType ?? CommunicateType.None;
            set
            {
                var v = Device?.CommunicateType ?? CommunicateType.None;
                if (Device is not null && SetProperty(ref v, value, nameof(CommunicateType)))
                {
                    Device.CommunicateType = v;
                }
            }
        }

        public string Alias
        {
            get => Device?.Alias;
            set
            {
                var v = Device.Alias ?? string.Empty;
                if (Device is not null && SetProperty(ref v, value, nameof(Alias)))
                {
                    Device.Alias = v;
                }
            }
        }

        public EmsDevice Device { get; private set; }


        public PointMapProxyViewModel(EmsDevice device)
        {
            #region event regists
            ViewSwitchCommand = new AsyncRelayCommand(ViewRefresh);
            //MultipleCheckCommand = new AsyncRelayCommand<object>(ViewRefresh);
            //SingleCheckCommand = new AsyncRelayCommand<object>(ViewRefresh);
            ImportPointMap_FunctionCode_04_Command = new AsyncRelayCommand(ImportPointMap_FunctionCode_04);
            ImportPointMap_FunctionCode_03_Command = new AsyncRelayCommand(ImportPointMap_FunctionCode_03);
            RowEditCommand = new AsyncRelayCommand(RowEdit);
            #endregion

            Identifier = "RootDialog";
            CacheLength = 125;
            Device = device;
            //if (device.PointMap is null)
            //{
            //    Device.PointMap = new EmsPointMap();
            //}

            //_pointMap = device.PointMap;
            //if (_pointMap.EmsPoints is null)
            //{
            //    _pointMap.EmsPoints = new List<EmsPoint>();
            //}

            //ViewRefresh();
        }

        public IAsyncRelayCommand ViewSwitchCommand { get; set; }
        public async Task ViewRefresh()
        {
            if (_pointMap?.EmsPoints?.Any() != true)
            {
                return;
            }

            ClearData();

            await Task.Delay(TimeSpan.FromSeconds(1))
                .ContinueWith(
                    (t1, _) =>
                    {
                        if (MultipleGroups)
                        {
                            MultipleCheck();
                        }
                        else
                        {
                            SingleCheck();
                        }

                    },
                    null, TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(
                    (t2, _) =>
                    {
                        if (MultipleGroups)
                        {
                            ViewLoading();
                        }
                        else
                        {
                            PointMaps.SingleViewLoading();
                        }

                        if (DialogHost.IsDialogOpen(Identifier))
                        {
                            DialogHost.Close(Identifier);
                        }
                    },
                    null,
                    TaskScheduler.FromCurrentSynchronizationContext());

        }
        //public readonly IAsyncRelayCommand<object> MultipleCheckCommand;
        private void MultipleCheck()
        {
            if (Content is not null)
            {
                (Content as UserControl).DataContext = null;
                Content = null;

                GC.Collect();
            }

            Content = new PointMapMultipleControl(this);
        }

        //public readonly IAsyncRelayCommand<object> SingleCheckCommand;
        private void SingleCheck()
        {
            if (Content is not null)
            {
                (Content as UserControl).DataContext = null;
                Content = null;

                GC.Collect();
            }

            PointMaps = new PointMapViewModel("All", _pointMap.EmsPoints, Identifier);
            Content = new PointMapSingleControl(PointMaps);

        }

        public IAsyncRelayCommand ImportPointMap_FunctionCode_04_Command { get; set; }
        private async Task ImportPointMap_FunctionCode_04()
        {
            await ImportProcess(ModbusFunctionCode.ReadInputRegisters);
        }

        public IAsyncRelayCommand ImportPointMap_FunctionCode_03_Command { get; set; }
        private async Task ImportPointMap_FunctionCode_03()
        {
            await ImportProcess(ModbusFunctionCode.ReadHoldingRegisters);
        }

        private async Task ImportProcess(ModbusFunctionCode functionCode)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(.xlsx)|*.xlsx";
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            var points = ExcelHelper.ImportToEmsPoint(ofd.FileName, (int)functionCode);
            foreach (var point in points)
            {
                var target = _pointMap.EmsPoints.FirstOrDefault(
                    f => f.FunctionCode == point.FunctionCode
                    && f.GroupName == point.GroupName
                    && f.Address == point.Address);
                if (target != null)
                {
                    var index = _pointMap.EmsPoints.IndexOf(target);
                    _pointMap.EmsPoints[index] = point;
                }
                else
                {
                    _pointMap.EmsPoints.Add(point);
                }
            }

            await ViewRefresh();

            await EmsSolutionContext.SaveEmsSolution(EmsSolutionContext.Current);
        }

        public IAsyncRelayCommand RowEditCommand { get; set; }
        private async Task RowEdit()
        {

        }

        private void ViewLoading()
        {
            PointMaps = null;

            if (PointMapsOf4?.Any() != true)
            {
                var fpg4 = _pointMap.EmsPoints.Where(w => w.FunctionCode == ModbusFunctionCode.ReadInputRegisters);
                PointMapsOf4 = [.. fpg4
                .GroupBy(g => g.GroupName)
                .Select(
                    s => new PointMapViewModel(
                        s.Key, [.. s.OfType<EmsPoint>()],Identifier))];
                foreach (var item in PointMapsOf4)
                {
                    item.SingleViewLoading();
                }

                SelectedPointMapOf4 = PointMapsOf4.First();
            }

            if (PointMapsOf3?.Any() != true)
            {
                var fpg3 = _pointMap.EmsPoints.Where(w => w.FunctionCode == ModbusFunctionCode.ReadHoldingRegisters);

                PointMapsOf3 = [.. fpg3
                .GroupBy(g => g.GroupName)
                .Select(
                    s => new PointMapViewModel(
                        s.Key, [.. s.OfType<EmsPoint>()],Identifier))];
                foreach (var item in PointMapsOf3)
                {
                    item.SingleViewLoading();
                }

                SelectedPointMapOf3 = PointMapsOf3.First();
            }
        }

        private void ClearData()
        {
            PointMaps = null;
            SelectedPointMapOf3 = null;
            PointMapsOf3 = null;
            SelectedPointMapOf4 = null;
            PointMapsOf4 = null;
        }
    }
}
