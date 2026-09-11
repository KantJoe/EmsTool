using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using org.Communication;
using org.Models.Messagings;
using org.Ui.Poseidon.Views;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TouchSocket.Mqtt;
using org.Utils;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MqttSimulateDetailViewModel : ObservableObject
    {
        public MqttTcpClientObject Client { get; private set; }

        [ObservableProperty]
        private bool _expand;
        [ObservableProperty]
        private bool _online;

        #region options
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _account;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _port;

        [ObservableProperty]
        private string _protocol;

        [ObservableProperty]
        private string _ip;

        [ObservableProperty]
        private string _sn;

        public string ClientId
        {
            get => Client?.InnerOptions?.ClientId;
        }
        #endregion

        [ObservableProperty]
        private ObservableCollection<MqttTopicViewModel> _topics;

        [ObservableProperty]
        private MqttTopicViewModel _topic;

        [ObservableProperty]
        private ObservableCollection<MqttMessageViewModel> _contents;

        [ObservableProperty]
        private ObservableCollection<MqttWritableDataViewModel> _writableDatas;

        [ObservableProperty]
        private MqttWritableDataViewModel _selectedData;

        public MqttSimulateDetailViewModel(string sn, MqttTcpClientObject mqttClient)
        {
            ConnectCommand = new AsyncRelayCommand(Connect);
            DisConnectCommand = new AsyncRelayCommand(DisConnect);
            AddSubscriptionCommand = new AsyncRelayCommand(AddSubscription);
            ClearContentCommand = new AsyncRelayCommand(ClearContent);
            EditRegistersCommand = new AsyncRelayCommand<object>(EditRegisters);

            Client = mqttClient;
            SetOptions();
            Online = mqttClient.Online;

            Contents = [];
            Topics = [.. mqttClient.Topics?.Select(s => new MqttTopicViewModel(s.Topic, (int)s.QosLevel)) ?? Array.Empty<MqttTopicViewModel>()];
            WritableDatas = [];
        }

        public IAsyncRelayCommand AddSubscriptionCommand { get; set; }
        private async Task AddSubscription()
        {

        }

        public IAsyncRelayCommand ConnectCommand { get; set; }
        private async Task Connect()
        {
            if (string.IsNullOrEmpty(Sn)
                || string.IsNullOrEmpty(Protocol)
                || string.IsNullOrEmpty(Ip)
                || string.IsNullOrEmpty(Port)
                || string.IsNullOrEmpty(Account)
                || string.IsNullOrEmpty(Password))
            {
                return;
            }

            Topic = null;
            Topics.Clear();
            Contents.Clear();

            Client.Sn = Sn;
            var config = Client.OriginConfig;
            config.Protocol = Protocol;
            config.Ip = Ip;
            config.Port = Port;
            config.Account = Account;
            config.Password = Password;

            await CommunicateAdapterPool.AddMqttClientAsync(Client);
            await Client?.DisconnectAsync();
            await Client.ConnectAsync();

            Online = Client?.Online ?? false;
            Topics = [.. Client.Topics?.Select(s => new MqttTopicViewModel(s.Topic, (int)s.QosLevel)) ?? Array.Empty<MqttTopicViewModel>()];
            WritableDatas.Clear();
            Expand = false;


        }

        public async Task ReceiveMessage(RawMqttMessage msg)
        {
            try
            {
                if (!Online)
                {
                    return;
                }

                var sb = new StringBuilder();
                foreach (var item in msg.Buffers)
                {
                    var keys = item.Key.Split("_");
                    var range = keys.Last().Split("~");
                    var start = range.First();
                    var end = range.Last();
                    sb.Append($"{start.ToString().PadLeft(4, '0')} - {end.ToString().PadLeft(4, '0')}: \n");
                    sb.Append(item.Value.ToArray().ToLogString() + "\n");
                }

                Contents.Add(new MqttMessageViewModel()
                {
                    Topic = msg.Topic,
                    Qos = msg.QosLevel,
                    FunctionCode = msg.FunctionCode,
                    Text = sb.ToString(),
                    CreatedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (Exception ex)
            {

            }

        }

        public IAsyncRelayCommand DisConnectCommand { get; set; }
        private async Task DisConnect()
        {
            await Client?.DisconnectAsync();

            Online = Client?.Online ?? false;

        }

        public IAsyncRelayCommand ClearContentCommand { get; set; }
        private async Task ClearContent()
        {
            Contents.Clear();
        }

        public IAsyncRelayCommand EditRegistersCommand { get; set; }
        private async Task EditRegisters(object functionCode)
        {
            var needFix = false;
            var list = WritableDatas.OrderBy(o => o.Address).ToList();
            if (list?.Any() != true)
            {
                return;
            }

            var startAddress = list.First().Address;
            for (var index = 1; index < list.Count; index++)
            {
                if (list[index].Address != (list[index - 1].Address + 1))
                {
                    needFix = true;
                    list[index].Address = (ushort)(list[index - 1].Address + 1);
                }
            }

            if (needFix)
            {
                return;
            }

            var bytes = list.Select(s => s.Value).ToArray().GetBytesFromBigEndian();
            await Client.WriteHoldingRegisters(1, startAddress, bytes.AsMemory());
        }

        private void SetOptions()
        {
            var config = Client?.OriginConfig;
            if (config is null)
            {
                return;
            }

            Name = config.Environment + "@" + config.Ip;
            Sn = Client.Sn;
            Ip = config.Ip;
            Protocol = config.Protocol;
            Port = config.Port;
            Password = config.Password;
            Account = config.Account;
        }
    }
}
