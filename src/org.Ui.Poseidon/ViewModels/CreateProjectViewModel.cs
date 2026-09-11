using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Models;
using org.Ui.ViewModels;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class CreateProjectViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _projectName;

        [ObservableProperty]
        private ObservableCollection<EmsProtocolViewModel> _protocols;

        [ObservableProperty]
        private EmsProtocolViewModel _selectedProtocol;

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        private int _slaveId;

        [ObservableProperty]
        private string _portName;

        [ObservableProperty]
        private int _port;

        public CreateProjectViewModel()
        {
            CreateProjectCommand = new AsyncRelayCommand(CreateProject);


            Protocols = InitializeProtocols();
            SelectedProtocol = Protocols.First();

            SlaveId = 1;
            PortName = "COM99";
        }

        private ObservableCollection<EmsProtocolViewModel> InitializeProtocols()
        {
            string[] list = [EmsProtocol.Product01];
            var files = Directory.GetFiles(FilePathConst.DeviceModule_Directory, "org.*.dll", SearchOption.TopDirectoryOnly)
                .Select(s => Path.GetFileName(s).Replace("org.", "").Replace(".dll", ""));
            var vmList = list
                .Where(w => files.Any(a => a == w))
                .Select(s => new EmsProtocolViewModel()
                {
                    Display = s,
                    Value = s,
                });

            return [.. vmList];
        }

        public IAsyncRelayCommand CreateProjectCommand { get; set; }
        private async Task CreateProject()
        {
            if (ProjectName?.Length >=4 != true)
            {
                return;
            }

            var solution = EmsSolutionContext.CreateEmsSolution(SelectedProtocol.Value, ProjectName);
            var firstDevice = solution.DeviceTopologies.FirstOrDefault();
            var options = firstDevice?.ConnectionOptions ?? new EmsConnectionOption(); ;
            if (PortName.StartsWith("COM"))
            {
                options.SlaveId = (byte)SlaveId;
                options.PortName = PortName;
                options.Port = string.Empty;
            }
            else if (Regex.IsMatch(PortName, @"\d+.\d+.\d+.\d+"))
            {
                options.SlaveId = 0;
                options.PortName = PortName;
                options.Port = Port.ToString();
            }

            firstDevice.ConnectionOptions = options;

            if (AssemblyContext.Instance.LoadAssembley(SelectedProtocol.Value)
                && ModuleContext.Instance.ModulePool.TryGetValue(SelectedProtocol.Value, out var module))
            {
                solution.Settings = module.ResetEmsSetting();

                DialogResult = await EmsSolutionContext.SaveEmsSolution(solution);
            }
        }


    }

    public partial class EmsProtocolViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _display;

        [ObservableProperty]
        private string _value;

        [ObservableProperty]
        private bool _isEnabled;

    }
}
