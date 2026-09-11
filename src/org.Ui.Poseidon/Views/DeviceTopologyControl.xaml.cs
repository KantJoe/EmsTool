using CommunityToolkit.Mvvm.Messaging;
using org.Communication.ViewModels;
using org.Models.Messagings;
using org.Ui.Messagings;
using System.Windows.Controls;
using System.Windows.Documents;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// DeviceTopologyControl.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceTopologyControl : UserControl
    {
        static ushort[] EMPTY_BUFFER = new ushort[125];
        public DeviceTopologyViewModel Vm { get; private set; }
        public DeviceTopologyControl()
        {
            InitializeComponent();

            Vm = new DeviceTopologyViewModel();
            this.DataContext = Vm;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Vm.InitializeLinks();
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ClearRegisters();
        }

        private async void BtnQuickConnect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ClearRegisters();
            await Vm.ConnectAllCommand.ExecuteAsync(null);
        }

        private async void BtnQuickDisconnect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ClearRegisters();
            await Vm.DisconnectAllCommand.ExecuteAsync(null);
        }

        private async void CbLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearRegisters();
            if (string.IsNullOrEmpty(Vm.SelectedLink))
            {
                return;
            }

            WeakReferenceMessenger.Default.Register<DeviceTopologyControl, UshortMessage, string>
                (this, Vm.SelectedLink, async (obj, msg) =>
                {
                    obj.ReceiveMessage(msg);
                });
        }

        private void ClearRegisters()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            ReceiveMessage(new UshortMessage(EMPTY_BUFFER));
            ReceiveMessage(new UshortMessage(EMPTY_BUFFER));
        }

        public void ReceiveMessage<T>(T msg)where T:UshortMessage
        {
            this.Dispatcher.Invoke(() =>
            {
                if (this.Vm.EnergyMovingTopology is not UserControl uc
                    || uc.DataContext is not IRecipient<T> recipient)
                {
                    return ;
                }

                recipient.Receive(msg);
            });
            return ;
        }
    }
}
