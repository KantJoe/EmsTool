using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Priviledge
{
    public class ApplicationRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public static ApplicationRole None => new ApplicationRole { Id = string.Empty, Name = "None" };
    }
}
