using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Priviledge;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Privilege.ViewModels
{
    public partial class ApplicationRoleSelectedViewModel : ApplicationRoleViewModel
    {
        [ObservableProperty]
        private bool _selected;
        public ApplicationRoleSelectedViewModel(ApplicationRole role) : base(role)
        {
        }
    }
}
