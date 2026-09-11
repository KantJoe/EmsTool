using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Communication;
using org.Ui.Poseidon.Views;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MqttSimulateViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MqttSimulateDetailViewModel> _details;

        [ObservableProperty]
        private MqttSimulateDetailViewModel _selectedDetail;

        [ObservableProperty]
        private double _height;

        public double DefaultHeight = 670;

        public MqttSimulateViewModel()
        {
            DeleteCommand = new AsyncRelayCommand(Delete);
            AppendCommand = new AsyncRelayCommand(Append);

            Loaded();
        }

        /// <summary>
        /// 由于目前禁止增删，就无所谓刷新
        /// </summary>
        public void Loaded()
        {
            var existsClients = CommunicateAdapterPool.ModbusMasterPool
                .Where(w => w.Value is MqttTcpClientObject)
                .Select(s => new MqttSimulateDetailViewModel(
                    (s.Value as MqttTcpClientObject).Sn, s.Value as MqttTcpClientObject))
                ??Array.Empty<MqttSimulateDetailViewModel>();
            var emptyEnvs = ConfigContext.GenericConfig.MqttConfig.EnvironmentConfigs
                .Where(w => !existsClients.Any(a => a.Client.OriginConfig.Environment == w.Environment))
                .Select(s => new MqttSimulateDetailViewModel("", new MqttTcpClientObject() { OriginConfig = s }))
                ?? Array.Empty<MqttSimulateDetailViewModel>();

            Details = [.. existsClients, .. emptyEnvs];


            SelectedDetail = Details?.FirstOrDefault();
        }

        public IAsyncRelayCommand AppendCommand { get; set; }
        private async Task Append()
        {
            await UiGlobalContext.ShowRootDialog(new AppendMqttControl(), closedHandler: async (s, e) =>
            {
                if (e.Parameter is null)
                {
                    return;
                }


            });
        }

        public IAsyncRelayCommand DeleteCommand { get; set; }
        private async Task Delete()
        {

        }
    }
}
