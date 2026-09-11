using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class DatabaseConfig
    {
        public bool InitialDatabaseOnStart { get; set; }
        public List<DatabaseInstanceConfig> DatabaseInstances { get; set; }
    }

    public class DatabaseInstanceConfig
    {
        public string UniqueId { get; set; }

        public string DbType { get; set; }

        public string DbName { get; set; }

        public string ConnectionString { get; set; }

    }
}
