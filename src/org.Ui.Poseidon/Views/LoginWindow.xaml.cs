using org.Models.Priviledge;
using org.Ui.Poseidon.ViewModels;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginWindowViewModel _vm;

        public LoginWindow()
        {
            InitializeComponent();

            var current = Assembly.GetExecutingAssembly();
            var fileVersion = current.GetCustomAttribute<AssemblyFileVersionAttribute>();
            this.TbTitle.Text = (ConfigContext.GenericConfig.MainTitle + "_V" + ConfigContext.GenericConfig.Version);

            PicMain.Source = BitmapFrame.Create(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Resources\\Images\\favicon.ico"), UriKind.Relative));

            this.CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Close,
                (t, e) => { this.DialogResult = false; },
                (s, e) => { e.CanExecute = true; }));

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

            var targetFile = EmsSolutionContext.Files
                .FirstOrDefault(f => System.IO.Path.GetFileNameWithoutExtension(f) == _vm.SelectedProject);
            EmsSolutionContext.SetSolution(targetFile);

            this.DialogResult = true;
        }

    }
}
