using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui
{
    public partial class UiGlobalContext : ObservableObject
    {
        [ObservableProperty]
        private bool _online;
        [ObservableProperty]
        private byte _mainSlaveId;

        public bool IsLivingTraced { get; set; }

        public static UiGlobalContext Instance { get; private set; }

        private UiGlobalContext()
        {
            Online = false;
            MainSlaveId = 1;
        }
        static UiGlobalContext()
        {
            Instance = new UiGlobalContext();
        }

        public const string RootDialog = "RootDialog";

        public static Action<string> EnqueueRootMessage { get; set; }

        public static async Task ShowRootDialog(object content,
            DialogOpenedEventHandler openHandler = null,
            DialogClosingEventHandler closingHandler = null,
            DialogClosedEventHandler closedHandler = null)
        {
            await Task.Factory.StartNew(
                async () =>
                {
                    await DialogHost.Show(
                        content, RootDialog, openHandler, closingHandler, closedHandler);
                },
                CancellationToken.None,
                TaskCreationOptions.RunContinuationsAsynchronously,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static void CloseRootDialog(object paramater = null)
        {
            DialogHost.Close(RootDialog, paramater);
        }
    }
}
