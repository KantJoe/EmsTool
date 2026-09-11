using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Priviledge;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Privilege.ViewModels
{
    public partial class ApplicationResourceViewModel : ObservableObject
    {
        public ApplicationResource Resource { get; private set; }
        [ObservableProperty]
        private string _name;

        public PriviledgeLevel PriviledgeLevel
        {
            get
            {
                return (PriviledgeLevel)Resource?.GetType()?.GetProperty(Name)?.GetValue(Resource);
            }
            set
            {
                var v = PriviledgeLevel.None;
                if (SetProperty(ref v, value))
                {
                    var p = Resource?.GetType()?.GetProperty(Name);
                    p.SetValue(Resource, v);
                }
            }
        }

        public ApplicationResourceViewModel(
            ApplicationResource applicationResource, string name)
        {
            Resource = applicationResource;
            Name = name;
        }
    }
}
