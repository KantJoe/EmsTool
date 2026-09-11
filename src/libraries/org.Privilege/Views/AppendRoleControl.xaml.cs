using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using org.Models.Priviledge;
using org.Privilege.ViewModels;
using org.Ui;
using org.Ui.MultiLanguage;
using org.Ui.ViewModels;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace org.Privilege.Views
{
    /// <summary>
    /// AppendRoleControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppendRoleControl : UserControl
    {
        private DialogParams _dialogParams;
        private ApplicationRole _role;
        private AppendRoleViewModel _vm;
        public AppendRoleControl(DialogParams dialogParams)
        {
            InitializeComponent();

            _dialogParams = dialogParams;
            _vm = new AppendRoleViewModel();
            DataContext = _vm;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _dialogParams.DialogReulst = DialogReulst.NoChanged;
            DialogHost.Close(UiGlobalContext.RootDialog, _dialogParams);
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (LoginAccountContext.ApplicationRoles
                .Exists(e => e.Id == _vm.Name || e.Name == _vm.Name))
            {
                MessageBox.Show(MultiLang.GetString(MultiLang.Instance.系统提示_存在重复名称));
                return;
            }

            _role = new ApplicationRole
            {
                Id = _vm.Name,
                Name = _vm.Name,
            };

            _dialogParams.UpdatedEntity = _role;
            _dialogParams.DialogReulst = DialogReulst.Succeed;
            DialogHost.Close(UiGlobalContext.RootDialog, _dialogParams);
        }
    }

    public partial class AppendRoleViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;
    }
}
