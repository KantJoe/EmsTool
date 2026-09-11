using org.Models;
using org.Models.Priviledge;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Utils.Global
{
    public class LoginAccountContext
    {
        public const string DEFAULT_ACCOUNT = "admin";
        public static string[] DEFAULT_ROLES => new[] { "admin", "Operator", "Maintainer", "Installer" };
        public static LoginAccountInfo CurrentAccount { get; private set; }

        public static ApplicationResource AggregatePriviledges { get; private set; }

        public static List<LoginAccount> LoginAccounts { get; private set; }
        public static List<ApplicationRole> ApplicationRoles { get; private set; }
        public static List<AccountAndRoleMapping> AccountAndRoleMappings { get; private set; }
        public static ConcurrentDictionary<string, ApplicationResource> RolePriviledges { get; private set; }

        static LoginAccountContext()
        {
            LoginAccounts = new List<LoginAccount>();
            ApplicationRoles = new List<ApplicationRole>();
            AccountAndRoleMappings = new List<AccountAndRoleMapping>();
            RolePriviledges = new ConcurrentDictionary<string, ApplicationResource>();
        }
        public static bool Login(LoginAccount loginAccount)
        {
            var physicalLoginAccount = LoginAccounts
                .FirstOrDefault(
                    e => e.Account == loginAccount?.Account
                    && SecurityUtil.VerifyInputToMd5Hash(
                        loginAccount?.Password, e.Password));
            if (physicalLoginAccount is null)
            {
                return false;
            }

            var accountAndRoles = AccountAndRoleMappings
                .Where(w => w.LoginAccountId == physicalLoginAccount.Id)
                .ToList();
            CurrentAccount = new LoginAccountInfo
            {
                AccountAndRoles = accountAndRoles,
                LoginAccount = physicalLoginAccount
            };

            Login_PriviledgesDistribute(accountAndRoles);

            return true;
        }

        public static void Logout()
        {
            CurrentAccount = LoginAccountInfo.Default;
        }

        public static IEnumerable<ApplicationResource> GetCurrentAccountPriviledges()
        {
            if (CurrentAccount?.AccountAndRoles?.Any() != true)
            {
                return new List<ApplicationResource>();
            }

            return RolePriviledges
                .Where(w => CurrentAccount.AccountAndRoles.Any(a => a.ApplicationRoleId == w.Key))
                .Select(sm => sm.Value);
        }

        public static void Initialize()
        {
            CurrentAccount = LoginAccountInfo.Default;
            LoadLoginAccounts();
            LoadApplicationRoles();
            LoadAccountAndRoles();
            LoadRolePriviledges();
        }

        public static bool AccountValidation(string account)
        {
            return account?.Length > 6
                && !LoginAccounts.Exists(e => e.Account == account);
        }

        private static void Login_PriviledgesDistribute(List<AccountAndRoleMapping> accountAndRoles)
        {
            var priviledge = new ApplicationResource();
            var currentAccountRoles = ApplicationRoles
                .Where(w => accountAndRoles
                    .Exists(e => e.ApplicationRoleId == w.Id))
                .Select(s => s.Name);
            var currentAccountResources = RolePriviledges
                .Where(w => currentAccountRoles.Any(a => a == w.Key))
                .Select(s => s.Value);

            var resourceProperties = priviledge.GetType()
                .GetProperties(System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public);

            foreach (var property in resourceProperties)
            {
                property.SetValue(
                    priviledge,
                    currentAccountResources
                        .Select(s => (PriviledgeLevel)s.GetType()
                            .GetProperty(property.Name).GetValue(s))
                        .Aggregate((accumulate, next) => accumulate | next));
            }

            AggregatePriviledges = priviledge;
        }

        public static void UpdateRolePriviledge(string roleId, ApplicationResource resource)
        {
            if (roleId == DEFAULT_ROLES[0])
            {
                return;
            }

            var fileName = Path.Combine(FilePathConst.PriviledgesDirectory, roleId + "_priviledges.role.json");
            if (resource is null)
            {
                RolePriviledges.Remove(roleId, out _);
                File.Delete(fileName);
            }
            else
            {
                RolePriviledges[roleId] = resource;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                    resource, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(fileName, json, Encoding.UTF8);
            }

        }
        private static void LoadRolePriviledges()
        {
            Parallel.ForEach(
                ApplicationRoles,
                new ParallelOptions() { MaxDegreeOfParallelism = 4 },
                item =>
                {
                    if (!DEFAULT_ROLES.Contains(item.Id))
                    {
                        RolePriviledges[item.Id] = new ApplicationResource();
                    }

                    var fileName = item.Id + "_priviledges.role.json";
                    fileName = Path.Combine(FilePathConst.PriviledgesDirectory, fileName);
                    if (!File.Exists(fileName))
                    {
                        LogFactory.Info(fileName + " not exists!!!");
                        return;
                    }

                    try
                    {
                        var json = File.ReadAllText(fileName, Encoding.UTF8);
                        RolePriviledges[item.Id]
                            = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationResource>(json);
                    }
                    catch (Exception ex)
                    {
                        LogFactory.Error(ex, "LoginAccountContext.LoadRolePriviledges {fileName}", fileName);
                    }
                });

            var priviledge = new ApplicationResource();
            var allPriviledges = typeof(ApplicationResource)
                .GetProperties(
                    System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public);
            foreach (var property in allPriviledges)
            {
                property.SetValue(priviledge, PriviledgeLevel.All);
            }

            RolePriviledges[DEFAULT_ACCOUNT] = priviledge;
            if (!RolePriviledges.ContainsKey(DEFAULT_ROLES[1]))
            {
                var operatorPriviledge = new ApplicationResource();
                foreach (var property in allPriviledges)
                {
                    property.SetValue(operatorPriviledge, PriviledgeLevel.All);
                }

                RolePriviledges[DEFAULT_ROLES[1]] = operatorPriviledge;
            }

            if (!RolePriviledges.ContainsKey(DEFAULT_ROLES[2]))
            {
                var maintainerPriviledge = new ApplicationResource();
                foreach (var property in allPriviledges)
                {
                    property.SetValue(maintainerPriviledge, PriviledgeLevel.Executable);
                }

                RolePriviledges[DEFAULT_ROLES[2]] = maintainerPriviledge;
            }

            if (!RolePriviledges.ContainsKey(DEFAULT_ROLES[3]))
            {
                var installerPriviledge = new ApplicationResource();
                foreach (var property in allPriviledges)
                {
                    property.SetValue(installerPriviledge, PriviledgeLevel.Visible);
                }

                RolePriviledges[DEFAULT_ROLES[3]] = installerPriviledge;
            }

        }

        public static void UpdateAccountAndRoles(string accountId,
            List<AccountAndRoleMapping> mappings,
            DialogAction action = DialogAction.Add)
        {
            AccountAndRoleMappings.RemoveAll(
                ra => ra.LoginAccountId == accountId);
            if (action != DialogAction.Remove)
            {
                AccountAndRoleMappings.AddRange(mappings);
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                AccountAndRoleMappings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePathConst.Priviledges_AccountRoles, json, Encoding.UTF8);
        }
        private static void LoadAccountAndRoles()
        {
            try
            {
                if (!File.Exists(FilePathConst.Priviledges_AccountRoles))
                {
                    LogFactory.Info(FilePathConst.Priviledges_AccountRolesName + " not exists!!!");
                    return;
                }

                var json = File.ReadAllText(FilePathConst.Priviledges_AccountRoles);
                AccountAndRoleMappings = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AccountAndRoleMapping>>(json)
                    ?? new List<AccountAndRoleMapping>();
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "LoginAccountContext.LoadApplicationRoles");
            }
            finally
            {
                if (!AccountAndRoleMappings.Exists(e => e.LoginAccountId == DEFAULT_ACCOUNT))
                {
                    AccountAndRoleMappings.Add(new AccountAndRoleMapping
                    {
                        LoginAccountId = DEFAULT_ACCOUNT,
                        ApplicationRoleId = DEFAULT_ACCOUNT
                    });
                }
            }
        }

        public static void UpdateApplicationRoles(ApplicationRole role, DialogAction action = DialogAction.Add)
        {
            ApplicationRoles.RemoveAll(ra => ra.Id == role.Id);
            if (action != DialogAction.Remove)
            {
                ApplicationRoles.Add(role);
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                ApplicationRoles, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePathConst.Priviledges_AppRoles, json, Encoding.UTF8);
        }
        private static void LoadApplicationRoles()
        {
            try
            {
                if (!File.Exists(FilePathConst.Priviledges_AppRoles))
                {
                    LogFactory.Info(FilePathConst.Priviledges_AppRolesName + " not exists!!!");
                    return;
                }

                var json = File.ReadAllText(FilePathConst.Priviledges_AppRoles);
                ApplicationRoles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApplicationRole>>(json)
                    ?? new List<ApplicationRole>();
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "LoginAccountContext.LoadApplicationRoles");
            }
            finally
            {
                foreach(var role in DEFAULT_ROLES)
                {
                    if (!ApplicationRoles.Exists(e => e.Id == role))
                    {
                        ApplicationRoles.Add(new ApplicationRole
                        {
                            Id = role,
                            Name = role
                        });
                    }
                }
            }
        }

        public static void UpdateLoginAccounts(LoginAccount account, DialogAction action = DialogAction.Add)
        {
            LoginAccounts.RemoveAll(ra => ra.Id == account.Id);
            if (action != DialogAction.Remove)
            {
                LoginAccounts.Add(account);
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                LoginAccounts, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePathConst.Priviledges_LoginAccounts, json, Encoding.UTF8);
        }
        private static void LoadLoginAccounts()
        {
            try
            {
                if (!File.Exists(FilePathConst.Priviledges_LoginAccounts))
                {
                    LogFactory.Info(FilePathConst.Priviledges_LoginAccountsName + " not exists!!!");
                    return;
                }

                var json = File.ReadAllText(FilePathConst.Priviledges_LoginAccounts);
                LoginAccounts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LoginAccount>>(json)
                    ?? new List<LoginAccount>();
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "LoginAccountContext.LoadLoginAccounts");
            }
            finally
            {
                if (!LoginAccounts.Exists(e => e.Account.ToLower() == DEFAULT_ACCOUNT))
                {
                    LoginAccounts.Add(new LoginAccount
                    {
                        Id = DEFAULT_ACCOUNT,
                        Account = DEFAULT_ACCOUNT,
                        Name = DEFAULT_ACCOUNT,
                        Password = ConfigContext.GenericConfig.AdminDefaultPwd
                    });
                }
            }
        }
    }
}
