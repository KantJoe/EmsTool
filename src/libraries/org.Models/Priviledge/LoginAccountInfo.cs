using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.Models.Priviledge
{
    public class LoginAccountInfo
    {
        public LoginAccount LoginAccount { get; set; }

        public List<AccountAndRoleMapping> AccountAndRoles { get; set; }


        [JsonIgnore]
        public static LoginAccountInfo Default => new LoginAccountInfo
        {
            LoginAccount = new LoginAccount
            {
                Id = "_",
                Account = "_",
                Name = "_"
            },
            AccountAndRoles = new List<AccountAndRoleMapping>()
        };
    }
}
