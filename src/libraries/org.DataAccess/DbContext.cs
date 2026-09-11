using org.DataAccess.Entities;
using org.Models;
using org.Utils.Global;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.DataAccess
{
    public class DbContext
    {
        private static Dictionary<string, SqlSugarScope> _dbs
            = new Dictionary<string, SqlSugarScope>()
            {
                {nameof(PoseidonDb),null }
            };
        public static DatabaseConfig DatabaseConfig => ConfigContext.GenericConfig?.DatabaseConfig;

        public static SqlSugarScope PoseidonDb => _dbs[nameof(PoseidonDb)];

        static DbContext()
        {
        }
        public static void Initialize()
        {
            foreach (var config in DatabaseConfig?.DatabaseInstances)
            {
                var dbType = (SqlSugar.DbType)Enum.Parse(typeof(SqlSugar.DbType), config.DbType);

                _dbs[config.DbName] = new SqlSugarScope(new ConnectionConfig()
                {
                    ConfigId=config.UniqueId,
                    IsAutoCloseConnection = true,
                    DbType = dbType,
                    ConnectionString = config.ConnectionString,
                    LanguageType = LanguageType.Default,//Set language
                },
                it =>
                {
                    // Logging SQL statements and parameters before execution
                    // 在执行前记录 SQL 语句和参数
                    it.Aop.OnLogExecuting = (sql, para) =>
                    {
                        LogFactory.Info(UtilMethods.GetNativeSql(sql, para));
                    };
                    it.Aop.OnError = ex =>
                    {
                        LogFactory.Error(ex, "Sql Executing Error ");
                    };
                });
            }

            InitialData();
        }

        public static void InitialData()
        {
            if (DatabaseConfig?.InitialDatabaseOnStart != true)
            {
                return;
            }

            PoseidonDb.DbMaintenance.CreateDatabase();

            PoseidonDb.CodeFirst.InitTables(typeof(Order));
        }
    }
}
