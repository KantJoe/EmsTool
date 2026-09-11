using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Models;
using org.Models.Messagings;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace org.Ui.ViewModels
{
    public partial class PointMapBaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _links;

        [ObservableProperty]
        private string _selectedLink;

        [ObservableProperty]
        private ObservableCollection<PointMapRangMappingViewModel> _mappings;

        [ObservableProperty]
        private PointMapRangMappingViewModel _selectedMapping;

        [ObservableProperty]
        private ObservableCollection<PointViewModel> _points;

        public IAsyncRelayCommand RefreshCommand { get; set; }
        public IAsyncRelayCommand<object> EditPointCommand { get; set; }
        public IAsyncRelayCommand StopCommand { get; set; }
        public IAsyncRelayCommand<object> AddAlertCommand { get; set; }
        public IAsyncRelayCommand<object> AddLivingMoniterCommand { get; set; }
        public PointMapBaseViewModel()
        {

            AddLivingMoniterCommand = new AsyncRelayCommand<object>(AddLivingMoniter);
        }

        protected virtual async Task AddLivingMoniter(object obj)
        {
            if (obj is not PointViewModel pointVM)
            {

                return;
            }

            var tracedPoint = new LivingTracedPoint
            {
                Key = SelectedLink,
                FunctionCode = (ModbusFunctionCode)pointVM.FunctionCode,
                Address = (ushort)pointVM.Address
            };
            LivingTracingContext.Instance.Points
                .RemoveAll(ra => ra.Key == tracedPoint.Key
                && ra.FunctionCode == tracedPoint.FunctionCode
                && ra.Address == tracedPoint.Address);
            LivingTracingContext.Instance.Points.Add(tracedPoint);
        }

        public virtual void Initialize()
        {
        }

        public virtual void ClearRegisters()
        {

        }

        protected virtual async void ReceiveMessage(PointMapBaseViewModel vm, UshortMessage msg)
        {
            if (msg.Mapping != $"{(int)SelectedMapping.FunctionCode}_{SelectedMapping.StartAddress}")
            {
                return;
            }

            var list = new List<PointViewModel>();
            for (var index = 0; index < msg.Buffer.Length; index++)
            {
                list.Add(new PointViewModel
                {
                    FunctionCode = SelectedMapping.FunctionCode,
                    Address = SelectedMapping.StartAddress + index,
                    CurrentValue = msg.Buffer.Slice(index, 1).ToArray().SingleOrDefault(),
                    UpdateDateTime = DateTime.Now.ToLongTimeString()
                });
            }

            Points = [.. list];

            Debug.WriteLine($"pointmap {msg.Mapping}");
        }

    }
}
