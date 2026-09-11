using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Priviledge;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Privilege.ViewModels
{
    public partial class LoginAccountViewModel : ObservableObject
    {
        public LoginAccount LoginAccount { get; set; }
        public List<ApplicationRole> ApplicationRoles { get; private set; }

        public string Account
        {
            get => LoginAccount?.Account ?? string.Empty;
            set
            {
                var v = LoginAccount?.Account ?? string.Empty;
                if (SetProperty(ref v, value))
                {
                    LoginAccount!.Account = value;
                }
            }
        }

        public string Password
        {
            get => LoginAccount?.Password ?? string.Empty;
            set
            {
                var v = LoginAccount?.Password ?? string.Empty;
                if (SetProperty(ref v, value))
                {
                    LoginAccount!.Password = value;
                }
            }
        }

        public string Name
        {
            get => LoginAccount?.Name ?? string.Empty;
            set
            {
                var v = LoginAccount?.Name ?? string.Empty;
                if (SetProperty(ref v, value))
                {
                    LoginAccount!.Name = value;
                }
            }
        }

        public string Roles
        {
            get => string.Join(", ", ApplicationRoles?.Select(s => s.Name));
            set
            {

            }
        }

        public LoginAccountViewModel(LoginAccount loginAccount, List<ApplicationRole> roles)
        {
            LoginAccount = loginAccount;
            ApplicationRoles = roles;
        }
    }
}
