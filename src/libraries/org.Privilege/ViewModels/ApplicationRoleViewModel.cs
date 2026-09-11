using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Priviledge;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Privilege.ViewModels
{
    public partial class ApplicationRoleViewModel : ObservableObject
    {
        public ApplicationRole Role { get; private set; }

        public string Name
        {
            get => Role?.Name ?? string.Empty;
            set
            {
                var v = Role?.Name ?? string.Empty;
                if (SetProperty(ref v, value))
                {
                    Role.Name = v;
                }
            }
        }

        public ApplicationRoleViewModel(ApplicationRole role)
        {
            Role = role;
        }
    }
}
