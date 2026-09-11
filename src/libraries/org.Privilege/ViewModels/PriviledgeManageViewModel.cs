using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Models;
using org.Models.Priviledge;
using org.Privilege.Views;
using org.Ui;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace org.Privilege.ViewModels
{
    public partial class PriviledgeManageViewModel : ObservableObject
    {
        public bool IsInitialed { get; private set; }
        [ObservableProperty]
        private ObservableCollection<LoginAccountViewModel> _loginAccounts;

        [ObservableProperty]
        private LoginAccountViewModel _selectedAccount;

        [ObservableProperty]
        private ObservableCollection<ApplicationRoleViewModel> _applicationRoles;

        [ObservableProperty]
        private ApplicationRoleViewModel _selectedRole;

        [ObservableProperty]
        private ObservableCollection<ApplicationRoleViewModel> _resource_ApplicationRoles;

        [ObservableProperty]
        private ApplicationRoleViewModel _resource_SelectedRole;

        [ObservableProperty]
        private ObservableCollection<ApplicationResourceViewModel> _applicationResources;
        [ObservableProperty]
        private ApplicationResourceViewModel _selectedResourced;

        public PriviledgeManageViewModel()
        {
            InitializeCommand = new AsyncRelayCommand(InitializeAsync);
            AppendAccountCommand = new AsyncRelayCommand(AppendAccount);
            EditAccountCommand = new AsyncRelayCommand<LoginAccountViewModel>(EditAccount);
            RemoveAccountCommand = new AsyncRelayCommand<LoginAccountViewModel>(RemoveAccount);
            AppendRoleCommand = new AsyncRelayCommand(AppendRole);
            RemoveRoleCommand = new AsyncRelayCommand<ApplicationRoleViewModel>(RemoveRole);
        }

        public IAsyncRelayCommand InitializeCommand { get; set; }
        public async Task InitializeAsync()
        {
            LoginAccounts = [..
                LoginAccountContext.LoginAccounts
                    .Select(s => new LoginAccountViewModel(s,SelectRoles(s)))];
            SelectedAccount = LoginAccounts.First();

            ApplicationRoles = [..
                LoginAccountContext.ApplicationRoles.Select(s=>new ApplicationRoleViewModel(s))];
            SelectedRole = ApplicationRoles.First();

            Resource_ApplicationRoles = [..
                LoginAccountContext.ApplicationRoles.Select(s=>new ApplicationRoleViewModel(s))];


            IsInitialed = true;
            await Task.Factory.StartNew(
                () =>
                {
                    DialogHost.Close(UiGlobalContext.RootDialog);
                },
                CancellationToken.None,
                TaskCreationOptions.RunContinuationsAsynchronously,
                TaskScheduler.FromCurrentSynchronizationContext());

        }

        [RelayCommand]
        private void Resource_SelectedRoleChanged()
        {
            if (Resource_SelectedRole is null)
            {
                return;
            }


            var resource = LoginAccountContext.RolePriviledges[Resource_SelectedRole.Role.Name];
            var resources = resource.GetType()
                .GetProperties(
                    System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public);
            ApplicationResources = [..
                resources.Select(s =>
                    new ApplicationResourceViewModel(resource, s.Name))];

        }

        private List<ApplicationRole> SelectRoles(LoginAccount loginAccount)
        {
            var matchedMaps = LoginAccountContext.AccountAndRoleMappings.Where(ww => ww.LoginAccountId == loginAccount.Id);
            return LoginAccountContext.ApplicationRoles
                .Where(w => matchedMaps.Any(a => a.ApplicationRoleId == w.Id))
                .ToList();
        }

        public IAsyncRelayCommand AppendAccountCommand { get; set; }
        private async Task AppendAccount()
        {
            await UiGlobalContext.ShowRootDialog(new AppendAccountControl(
                new DialogParams() { 
                    Action = DialogAction.Add }),
                closingHandler: new DialogClosingEventHandler((s, e) =>
                {
                    if (e.Parameter is not DialogParams param
                        || param?.DialogReulst != DialogReulst.Succeed)
                    {
                        return;
                    }

                    var account = param.UpdatedEntity as LoginAccountViewModel;
                    var accountId = account.LoginAccount.Id;
                    LoginAccountContext.UpdateLoginAccounts(account.LoginAccount);
                    LoginAccountContext.UpdateAccountAndRoles(accountId,
                        account.ApplicationRoles.Select(
                            s => new AccountAndRoleMapping
                            {
                                ApplicationRoleId = s.Id,
                                LoginAccountId = accountId
                            }).ToList(), DialogAction.Add);
                    LoginAccounts = [.. LoginAccountContext.LoginAccounts
                        .Select(s => new LoginAccountViewModel(s, SelectRoles(s)))];
                }));
        }

        public IAsyncRelayCommand<LoginAccountViewModel> EditAccountCommand { get; set; }
        private async Task EditAccount(LoginAccountViewModel accountVM)
        {
            if (accountVM.Account == LoginAccountContext.DEFAULT_ACCOUNT)
            {
                MessageBox.Show("系统账号无法编辑");
                return;
            }

            if (accountVM.Account == LoginAccountContext.CurrentAccount.LoginAccount.Account)
            {
                MessageBox.Show("当前登录账号,无法编辑");
                return;
            }

            await UiGlobalContext.ShowRootDialog(new AppendAccountControl(new DialogParams() { Action = DialogAction.Update,OriginEntity=accountVM }),
                closingHandler: new DialogClosingEventHandler((s, e) =>
                {
                    if (e.Parameter is not DialogParams param
                        || param?.DialogReulst != DialogReulst.Succeed)
                    {
                        return;
                    }

                    var account = param.UpdatedEntity as LoginAccountViewModel;
                    var accountId = account.LoginAccount.Id;
                    LoginAccountContext.UpdateLoginAccounts(account.LoginAccount,DialogAction.Update);
                    LoginAccountContext.UpdateAccountAndRoles(accountId,
                        account.ApplicationRoles.Select(
                            s => new AccountAndRoleMapping
                            {
                                ApplicationRoleId = s.Id,
                                LoginAccountId = accountId
                            }).ToList(), DialogAction.Update);
                    LoginAccounts = [.. LoginAccountContext.LoginAccounts
                        .Select(s => new LoginAccountViewModel(s, SelectRoles(s)))];
                }));
        }

        public IAsyncRelayCommand<LoginAccountViewModel> RemoveAccountCommand { get; set; }
        private async Task RemoveAccount(LoginAccountViewModel accountVM)
        {
            if (accountVM is null
                || accountVM.Account == LoginAccountContext.DEFAULT_ACCOUNT)
            {
                MessageBox.Show("默认账号,无法删除");
                return;
            }

            if ( accountVM.Account == LoginAccountContext.CurrentAccount.LoginAccount.Account)
            {
                MessageBox.Show("当前登录账号,无法删除");
                return;
            }

            LoginAccountContext.UpdateLoginAccounts(accountVM.LoginAccount, DialogAction.Remove);
            var accountId = accountVM.LoginAccount.Id;
            LoginAccountContext.UpdateAccountAndRoles(
                accountId,
                LoginAccountContext.ApplicationRoles.Select(
                            s => new AccountAndRoleMapping
                            {
                                ApplicationRoleId = s.Id,
                                LoginAccountId = accountId
                            }).ToList(),
                DialogAction.Remove);

            LoginAccounts = [.. LoginAccountContext.LoginAccounts
                        .Select(s => new LoginAccountViewModel(s, SelectRoles(s)))];
        }

        public IAsyncRelayCommand AppendRoleCommand { get; set; }
        private async Task AppendRole()
        {
            await UiGlobalContext.ShowRootDialog(new AppendRoleControl(new DialogParams() { Action = DialogAction.Add }),
                closingHandler: new DialogClosingEventHandler((s, e) =>
                {
                    if (e.Parameter is not DialogParams param
                        || param?.DialogReulst != DialogReulst.Succeed)
                    {
                        return;
                    }


                    var role = param.UpdatedEntity as ApplicationRole;
                    LoginAccountContext.UpdateApplicationRoles(role,DialogAction.Add);
                    LoginAccountContext.UpdateRolePriviledge(role.Id, new ApplicationResource());

                    ApplicationRoles = [.. LoginAccountContext.ApplicationRoles
                        .Select(s => new ApplicationRoleViewModel(s))];
                    Resource_ApplicationRoles = [..
                        LoginAccountContext.ApplicationRoles.Select(s=>new ApplicationRoleViewModel(s))];
                }));
        }

        public IAsyncRelayCommand<ApplicationRoleViewModel> RemoveRoleCommand { get; set; }
        private async Task RemoveRole(ApplicationRoleViewModel roleVM)
        {
            if (roleVM is null
                || LoginAccountContext.DEFAULT_ROLES.Contains(roleVM.Role.Id))
            {
                MessageBox.Show("默认角色,无法删除");
                return;
            }

            if (LoginAccountContext.AccountAndRoleMappings.Exists(e => e.ApplicationRoleId == roleVM.Role.Id))
            {
                MessageBox.Show($"{roleVM.Name} 仍作用在现有账号,无法删除");
                return;
            }

            LoginAccountContext.UpdateApplicationRoles(roleVM.Role, DialogAction.Remove);
            LoginAccountContext.UpdateRolePriviledge(roleVM.Role.Id, null);

            ApplicationRoles = [.. LoginAccountContext.ApplicationRoles
                .Select(s => new ApplicationRoleViewModel(s))];
            Resource_ApplicationRoles = [..
                LoginAccountContext.ApplicationRoles.Select(s=>new ApplicationRoleViewModel(s))];
        }

    }
}
