using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using org.Communication;
using org.Models.Messagings;
using org.Ui.Messagings;
using org.Ui.MultiLanguage;
using org.Ui.ViewModels;
using org.Ui.Views;
using org.Utils;
using org.Utils.Global;
using System.Collections.ObjectModel;

namespace org.Product01.ViewModels
{
    public partial class Product01SystemSummaryViewModel : SystemSummaryMessageBase
    {
        [ObservableProperty]
        private ObservableCollection<string> _links;

        [ObservableProperty]
        private string _selectedLink;


        public Product01SystemSummaryViewModel()
        {
            EnableWriteGeneratingCommand = new AsyncRelayCommand(EnableWriteGenerating);
            ResetGeneratingCommand = new AsyncRelayCommand(ResetGenerating);
            WriteGeneratingCommand = new AsyncRelayCommand(WriteGenerating);
            SelectLinkChangedCommand = new AsyncRelayCommand(SelectLinkChanged);
        }

        public IAsyncRelayCommand SelectLinkChangedCommand { get; set; }
        private async Task SelectLinkChanged()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);

            if (string.IsNullOrEmpty(SelectedLink))
            {
                return;
            }

            SelectedLinkChanged(SelectedLink);
            WeakReferenceMessenger.Default.Register<Product01SystemSummaryViewModel, UshortMessage, string>
                (this, SelectedLink, async (obj, msg) =>
                {
                    obj.Receive(msg);
                });
        }

        public void Load()
        {
            var list = CommunicateAdapterPool.ModbusMasterPool.Keys
                .ToList();
            Links = [.. list];
            MultiLang.OnLanguageChanged += SwitchLanguage;
        }

        public void Unload()
        {
            MultiLang.OnLanguageChanged -= SwitchLanguage;
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        public async void Receive(UshortMessage message)
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var link)
                || !link.Online)
            {
                // 保留连接状态下的有效数据，
                // 防止连接断开的‘无效帧’污染
                return;
            }

            OnReceivedMessage(message);
        }
        private void SwitchLanguage(string currentLanguage, string nextLanguage)
        {
            DissipationType = GetDissipationType(DTC);
            SystemStateText = GetSystemStateText(SystemState);
            RunningModeText = GetRunningMode(RunningMode);
            GridConnectedText = GetGridConnectedText(GridConnected);
            EmsStateText = GetEmsStateText(EmsState);
            InverStateText = GetInverStateText(InverState);
            GridStateText = GetGridStateText(GridState);
            BatteryStateText = GetBatteryStateText(BatteryState);
            ParallelStateText = GetParallelStateText(ParallelState);
            ParallelIdentityText = GetParallelIdentityText(ParallelIdentity);
            ParallelGridStateText = GetParallelGridStateText(ParallelGridState);
        }
    }
}
