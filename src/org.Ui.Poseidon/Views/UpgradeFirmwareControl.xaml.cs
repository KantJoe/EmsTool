using CommunityToolkit.Mvvm.Messaging;
using org.Ui.Messagings;
using org.Ui.Poseidon.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
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
    /// UpgradeFirmwareControl.xaml 的交互逻辑
    /// </summary>
    public partial class UpgradeFirmwareControl : UserControl
    {
        public UpgradeFirmwareViewModel VM;
        public UpgradeFirmwareControl()
        {
            InitializeComponent();
            VM = new UpgradeFirmwareViewModel();
            DataContext = VM;
        }

        private void ListViewScrollToEnd()
        {
            SvLogs.ScrollToEnd();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            VM.Unloaded();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VM.Loaded(ListViewScrollToEnd);
        }

        private async void CbLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            await VM.Receive(new Input00Message());
            WeakReferenceMessenger.Default.Register<UpgradeFirmwareControl, Input00Message, string>(this, VM.SelectedLink,
                 async (obj, msg) =>
                 {
                     await obj.Dispatcher.InvokeAsync(async () =>
                     {
                         await obj.VM.Receive(msg);
                     });
                 });
        }
    }
}
