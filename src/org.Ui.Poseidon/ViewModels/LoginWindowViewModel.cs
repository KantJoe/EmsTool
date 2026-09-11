using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Privilege.Views;
using org.Ui.MultiLanguage;
using org.Ui.Poseidon.Views;
using org.Ui.Views;
using org.Utils.Global;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class LoginWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _account;

        [ObservableProperty]
        private string _password;


        [ObservableProperty]
        private ObservableCollection<string> _projectNames;

        [ObservableProperty]
        private string _selectedProject;

        public LoginWindowViewModel()
        {
            SwitchLanguageCommand = new AsyncRelayCommand(SwitchLanguage);
            ImportProjectCommand = new AsyncRelayCommand(ImportProject);
            CreateProjectCommand = new AsyncRelayCommand(CreateProject);
            DeleteProjectCommand = new AsyncRelayCommand(DeleteProject);

            ProjectNames = [..EmsSolutionContext
                .Files.Select(Path.GetFileNameWithoutExtension)];


            Account = "admin";
            Password = "123456";
            SelectedProject = ProjectNames.FirstOrDefault();
        }


        public IAsyncRelayCommand ImportProjectCommand { get; set; }
        private async Task ImportProject()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Ems解决方案(*.emss)|*.emss";


            if (ofd.ShowDialog() != true || !File.Exists(ofd.FileName))
            {
                return;
            }

            var json = File.ReadAllText(ofd.FileName);
            if (await EmsSolutionContext.ImportEmsSolution(json))
            {
                EmsSolutionContext.Reinitialize();

                ProjectNames = new ObservableCollection<string>(
                    EmsSolutionContext.Files.Select(Path.GetFileNameWithoutExtension));
            }
        }

        [RelayCommand]
        private void ExportProject()
        {
            if (string.IsNullOrEmpty(SelectedProject))
            {
                return;
            }

            var fileName = Path.GetFileNameWithoutExtension(SelectedProject);
            OpenFolderDialog ofd = new OpenFolderDialog();

            if (ofd.ShowDialog() == true)
            {
                var targetFile = EmsSolutionContext.Files
                    .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == SelectedProject);

                File.Copy(targetFile,
                    Path.Combine(ofd.FolderName, fileName + FilePathConst.EmsSolution_FileExt));
            }
        }

        public IAsyncRelayCommand SwitchLanguageCommand { get; set; }
        private async Task SwitchLanguage()
        {
            MultiLang.SwitchLanguage();
        }

        public IAsyncRelayCommand CreateProjectCommand { get; set; }
        private async Task CreateProject()
        {
            var dialog = new CreateProjectWindow();
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                ProjectNames = [.. EmsSolutionContext
                    .Files.Select(Path.GetFileNameWithoutExtension)];
                SelectedProject = ProjectNames.FirstOrDefault(f => f == dialog.Vm.ProjectName);
            }
        }

        public IAsyncRelayCommand DeleteProjectCommand { get; set; }
        private async Task DeleteProject()
        {
            EmsSolutionContext.RemoveSolution(SelectedProject);
            ProjectNames = [.. EmsSolutionContext
                    .Files.Select(Path.GetFileNameWithoutExtension)];
        }
    }
}
