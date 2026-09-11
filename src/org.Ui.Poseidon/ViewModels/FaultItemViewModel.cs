using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using org.Utils;
using org.Ui.MultiLanguage;
using System.Windows.Media;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class FaultItemViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(No))]
        [NotifyPropertyChangedFor(nameof(EventCode))]
        [NotifyPropertyChangedFor(nameof(TriggedDateTime))]
        [NotifyPropertyChangedFor(nameof(Content))]
        [NotifyPropertyChangedFor(nameof(EventLevelStr))]
        [NotifyPropertyChangedFor(nameof(EventLevelBackground))]
        public partial ReadOnlyMemory<ushort> Buff { get; set; }

        public ushort No => Buff.GetPoint(0);
        public ushort EventCode => Buff.GetPoint(1);
        public string TriggedDateTime => Buff.GetStringFromBigEndian(2, 10)?.TrimEnd('\0');
        public string Content => Buff.GetStringFromBigEndian(12, 20)?.TrimEnd('\0');
        public string EventLevelStr => GetEventLevelStr();
        public Brush EventLevelBackground => GetEventLevelBackground();

        public FaultItemViewModel(ReadOnlyMemory<ushort> buff)
        {
            Buff = buff;
        }

        ~FaultItemViewModel()
        {
            Buff = default;
        }

        private Brush GetEventLevelBackground()
        {
            var code = Buff.GetPoint(1);
            if ((code >= 1000 && code <= 1075)
                || (code >= 3100 && code <= 5000))
            {
                return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Yellow);
        }

        private string GetEventLevelStr()
        {
            var code = Buff.GetPoint(1);
            if ((code >= 1000 && code <= 1075)
                || (code >= 3100 && code <= 5000))
            {
                return MultiLang.GetString("故障");
            }

            return MultiLang.GetString("警告");
        }
    }
}
