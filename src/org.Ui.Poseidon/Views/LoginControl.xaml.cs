using org.Models.Priviledge;
using org.Ui.Poseidon.ViewModels;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// LoginControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private LoginWindowViewModel _vm;
        public bool? DialogResult { get; private set; }

        public LoginControl()
        {
            InitializeComponent();

            var current = Assembly.GetExecutingAssembly();
            var fileVersion = current.GetCustomAttribute<AssemblyFileVersionAttribute>();
            this.TbTitle.Text = (ConfigContext.GenericConfig.MainTitle + "_V" + ConfigContext.GenericConfig.Version);

            //PicMain.Source = BitmapFrame.Create(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Resources\\Images\\favicon.ico"), UriKind.Relative));

            _vm = new LoginWindowViewModel();
            DataContext = _vm;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_vm.Account))
            {
                MessageBox.Show("账号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_vm.SelectedProject))
            {
                MessageBox.Show("项目不能为空");
                return;
            }

            var account = new LoginAccount() { Account = _vm.Account, Password = TxtPasswd.Password };

            if (!LoginAccountContext.Login(account))
            {
                MessageBox.Show("密码错误");
                return;
            }

            try
            {
                var targetFile = EmsSolutionContext.Files
                    .FirstOrDefault(f => System.IO.Path.GetFileNameWithoutExtension(f) == _vm.SelectedProject);
                EmsSolutionContext.SetSolution(targetFile);

                this.DialogResult = true;
                DialogHost.Close(UiGlobalContext.RootDialog, true);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "LoginControl.BtnLogin_Click project: " + _vm?.SelectedProject);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            DialogHost.Close(UiGlobalContext.RootDialog, false);
            Application.Current.MainWindow.Close();
        }
    }
}
