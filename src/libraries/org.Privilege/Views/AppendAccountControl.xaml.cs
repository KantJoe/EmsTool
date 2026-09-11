using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using org.Models.Priviledge;
using org.Privilege.ViewModels;
using org.Ui;
using org.Ui.MultiLanguage;
using org.Ui.ViewModels;
using org.Utils;
using org.Utils.Global;
using Flurl.Util;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// AppendAccountControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppendAccountControl : UserControl
    {
        private AppendAccountViewModel _vm;
        private DialogParams _dialogParams;
        private LoginAccount _account;
        private LoginAccountViewModel _entity;
        public AppendAccountControl(DialogParams dialogParams)
        {
            InitializeComponent();

            _dialogParams = dialogParams;

            _entity = dialogParams.OriginEntity as LoginAccountViewModel;
            _vm = new AppendAccountViewModel(_entity?.Account);
            _account = _entity?.LoginAccount;

            if (dialogParams.Action == DialogAction.Update)
            {
                _vm.Editable = false;
                _vm.Name = _entity.Name;

                foreach (var item in _vm.Roles)
                {
                    if (_entity.ApplicationRoles.Exists(e => e.Id == item.Role.Id))
                    {
                        item.Selected = true;
                    }
                }
            }

            DataContext = _vm;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.Password) && _vm.Password.Length < 6)
            {
                MessageBox.Show("密码太短");
                return;
            }

            var allAccounts = LoginAccountContext.LoginAccounts
                .Where(
                    w => _dialogParams.Action == DialogAction.Update
                    && w.Id != _entity.LoginAccount.Id);
            if (allAccounts.Any(e => e.Id == _vm.Account || e.Account == _vm.Account))
            {
                MessageBox.Show(MultiLang.GetString(MultiLang.Instance.系统提示_存在重复账号));
                return;
            }

            var updatedEntity = _dialogParams.Action == DialogAction.Add ?
                new LoginAccountViewModel(new LoginAccount
                {
                    Id = _vm.Account,
                    Account = _vm.Account
                },
                new List<ApplicationRole>()) : _entity;
            updatedEntity.ApplicationRoles.Clear();
            updatedEntity.ApplicationRoles
                .AddRange(_vm.Roles.Where(w => w.Selected)
                .Select(s => s.Role).ToList());

            updatedEntity.Name = _vm.Name;
            if (!string.IsNullOrEmpty(_vm.Password))
            {
                updatedEntity.Password = SecurityUtil.Md5(_vm.Password);
            }

            _dialogParams.UpdatedEntity = updatedEntity;
            _dialogParams.DialogReulst = DialogReulst.Succeed;
            DialogHost.Close(UiGlobalContext.RootDialog, _dialogParams);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _dialogParams.DialogReulst = DialogReulst.NoChanged;
            DialogHost.Close(UiGlobalContext.RootDialog, _dialogParams);
        }
    }

    public partial class AppendAccountViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _account;
        [ObservableProperty]
        private bool _editable;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private ObservableCollection<ApplicationRoleSelectedViewModel> _roles;

        public AppendAccountViewModel(string account)
        {
            Account = account;
            Editable = true;
            var roles = LoginAccountContext.ApplicationRoles
                .Select(s => new ApplicationRoleSelectedViewModel(s))
                .ToList();
            if (Account != LoginAccountContext.DEFAULT_ACCOUNT)
            {
                roles.RemoveAll(ra => ra.Role.Id == LoginAccountContext.DEFAULT_ACCOUNT);
            }
            Roles = [.. roles];
        }
    }
}
