using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Priviledge
{
    public class RoleAndResourceAndPriviledgeMapping
    {
        public string ApplicationRoleId { get; set; }

        public string ApplicationResource { get; set; }

        public PriviledgeLevel PriviledgeLevel { get; set; }
    }
}
