using CommunityToolkit.Mvvm.Messaging;
using org.Communication;
using org.Models.Messagings;
using org.Ui.Poseidon.ViewModels;
using ScottPlot.Interactivity;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    /// MqttSimulateControl.xaml 的交互逻辑
    /// </summary>
    public partial class MqttSimulateControl : UserControl
    {
        public MqttSimulateViewModel Vm;
        public MqttSimulateControl()
        {
            InitializeComponent();

            Vm = new MqttSimulateViewModel();
            DataContext = Vm;

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var height = Application.Current.MainWindow.Height * 0.78;
            Vm.Height = Application.Current.MainWindow.WindowState == WindowState.Maximized ? height : 670f;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var height = Application.Current.MainWindow.Height * 0.78;
            Vm.Height = Application.Current.MainWindow.WindowState == WindowState.Maximized ? height : 670f;

        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Vm.Details.Any(a => a.Name != Vm.SelectedDetail.Name && a.Ip == Vm.SelectedDetail.Ip))
            {
                return;
            }

            var selectedKey = Vm.SelectedDetail.Ip + "_" + Vm.SelectedDetail.Port;
            WeakReferenceMessenger.Default.UnregisterAll(this);
            WeakReferenceMessenger.Default.Register<MqttSimulateControl, RawMqttMessage, string>(this, selectedKey,
                 async (obj, msg) =>
                 {
                     await obj.Dispatcher.InvokeAsync(async () =>
                     {
                         var detail = obj.Vm.Details.FirstOrDefault(f => f.Sn == msg.Sn);
                         if (detail is null)
                         {
                             return;
                         }

                         await detail.ReceiveMessage(msg);
                     });
                 });

            await Vm.SelectedDetail.ConnectCommand.ExecuteAsync(null);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // 脱离焦点时不注销订阅
        }

        private async void BtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            var selectedKey = Vm.SelectedDetail.Ip + "_" + Vm.SelectedDetail.Port;
            WeakReferenceMessenger.Default.UnregisterAll(this);

            await Vm.SelectedDetail.DisConnectCommand.ExecuteAsync(null);
        }
    }
}
