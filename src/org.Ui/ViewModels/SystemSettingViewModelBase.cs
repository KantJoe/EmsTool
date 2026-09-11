using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Ui.MultiLanguage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace org.Ui.ViewModels
{
    public partial class SystemSettingViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _links;

        [ObservableProperty]
        private string _selectedLink;

        [ObservableProperty]
        private ObservableCollection<string> _periodTimers;

        [ObservableProperty]
        private string _selectedTimer;

        [ObservableProperty]
        private ObservableCollection<TimeSlot48ViewModel> _timeSlots48;

        [ObservableProperty]
        private TimeSlot48ViewModel _selectedTimeSlot48;

        public SystemSettingViewModelBase()
        {
            ResetSystemParam01Command = new AsyncRelayCommand(ResetSystemParam01);
            QuerySystemParam01Command = new AsyncRelayCommand<object>(QuerySystemParam01);
            WriteSystemParam01Command = new AsyncRelayCommand(WriteSystemParam01);
            RebootSystemCommand = new AsyncRelayCommand(RebootSystem);
            ResetSystemFaultsCommand = new AsyncRelayCommand(ResetSystemFaults);
            FactoryReset01Command = new AsyncRelayCommand(FactoryReset01);
            FactoryReset02Command = new AsyncRelayCommand(FactoryReset02);
            FactoryReset03Command = new AsyncRelayCommand(FactoryReset03);
            ReadInfoCommand = new AsyncRelayCommand(ReadInfo);

            ResetSystemParam02Command = new AsyncRelayCommand(ResetSystemParam02);
            QuerySystemParam02Command = new AsyncRelayCommand<object>(QuerySystemParam02);
            WriteSystemParam02Command = new AsyncRelayCommand(WriteSystemParam02);

            Reset48TimeSlotCommand = new AsyncRelayCommand(Reset48TimeSlot);
            Query48TimeSlotDataCommand = new AsyncRelayCommand<object>(Query48TimeSlotData);
            Write48TimeSlotCommand = new AsyncRelayCommand(Write48TimeSlot);

            PeriodTimerInitialize();
        }



        #region system param01
        public IAsyncRelayCommand ResetSystemParam01Command { get; set; }
        protected virtual async Task ResetSystemParam01()
        {

        }

        public IAsyncRelayCommand<object> QuerySystemParam01Command { get; set; }
        protected virtual async Task QuerySystemParam01(object lockObj)
        {

        }
        public IAsyncRelayCommand WriteSystemParam01Command { get; set; }
        protected virtual async Task WriteSystemParam01()
        {

        }

        public IAsyncRelayCommand RebootSystemCommand { get; set; }
        protected virtual async Task RebootSystem()
        {

        }

        public IAsyncRelayCommand ResetSystemFaultsCommand { get; set; }
        protected virtual async Task ResetSystemFaults()
        {

        }

        public IAsyncRelayCommand FactoryReset01Command { get; set; }
        protected virtual async Task FactoryReset01()
        {

        }

        public IAsyncRelayCommand FactoryReset02Command { get; set; }
        protected virtual async Task FactoryReset02()
        {

        }

        public IAsyncRelayCommand FactoryReset03Command { get; set; }
        protected virtual async Task FactoryReset03()
        {

        }

        public IAsyncRelayCommand ReadInfoCommand { get; set; }
        protected virtual async Task ReadInfo()
        {

        }
        #endregion

        #region system param02
        public IAsyncRelayCommand ResetSystemParam02Command { get; set; }
        protected virtual async Task ResetSystemParam02()
        {

        }

        public IAsyncRelayCommand<object> QuerySystemParam02Command { get; set; }
        protected virtual async Task QuerySystemParam02(object lockObj)
        {

        }
        public IAsyncRelayCommand WriteSystemParam02Command { get; set; }
        protected virtual async Task WriteSystemParam02()
        {

        }


        #endregion

        #region 48 period time
        public IAsyncRelayCommand Reset48TimeSlotCommand { get; set; }
        protected virtual async Task Reset48TimeSlot()
        {
            Debug.WriteLine("base");
        }

        public IAsyncRelayCommand<object> Query48TimeSlotDataCommand { get; set; }
        protected virtual async Task Query48TimeSlotData(object lockObj)
        {

        }

        public IAsyncRelayCommand Write48TimeSlotCommand { get; set; }
        protected virtual async Task Write48TimeSlot()
        {

        }

        protected virtual void TimeSlots48Initialize()
        {
            var counts = Enumerable.Range(1, 48);
            TimeSlots48 = [..counts.Select(s=>new TimeSlot48ViewModel {
                Time = GetTimeStr(s),
                Value=0
            })];
        }

        private string GetTimeStr(int index)
        {
            var tsStart = new TimeSpan(0, 0, (index - 1) * 30, 0);
            var tsEnd = new TimeSpan(0, 0, (index) * 30, 0);
            return $"{tsStart.Hours.ToString().PadLeft(2, '0')}:{tsStart.Minutes.ToString().PadLeft(2, '0')}-{tsEnd.Hours.ToString().PadLeft(2,'0')}:{tsEnd.Minutes.ToString().PadLeft(2, '0')}";
        }

        protected virtual void PeriodTimerInitialize()
        {
            var selectedIndex = PeriodTimers?.IndexOf(SelectedTimer) ?? 0;

            PeriodTimers = [MultiLang.GetString(MultiLang.Instance.无效), "1", "2", "3", "4"];
            SelectedTimer = PeriodTimers[selectedIndex > 0 ? selectedIndex : 0];
        }
        #endregion
        public virtual void Load()
        {
            MultiLang.OnLanguageChanged += SwitchLanguage;


            TimeSlots48Initialize();
            SelectedTimer = PeriodTimers.First();
        }

        public virtual void Unload()
        {
            MultiLang.OnLanguageChanged -= SwitchLanguage;
        }

        protected virtual void SwitchLanguage(string currentLanguage, string nextLanguage)
        {
            PeriodTimerInitialize();
        }
    }
}
