using CommunityToolkit.Mvvm.Messaging;
using org.Communication;
using org.Ui.Poseidon.ViewModels;
using org.Ui.ViewModels;
using org.Utils.Global;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// PointMapControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointMapBusControl : UserControl
    {
        private PointMapBaseViewModel _vm;
        public PointMapBusControl()
        {
            InitializeComponent();

            var solution = EmsSolutionContext.Current;
            var protocol = solution?.DeviceTopologies?.FirstOrDefault()?.EmsProtocol;
            var module = ModuleContext.Instance.GetModule(protocol);
            if (module is IMenuViewModel modelVM)
            {
                _vm = modelVM.PointMapInitialize();

                DataContext = _vm;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm?.Initialize();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _vm?.ClearRegisters();
        }

        private void CbLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        static ushort[] EmptyBuffer = new ushort[125];
        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await _vm?.RefreshCommand?.ExecuteAsync(null);
        }

    }
}
