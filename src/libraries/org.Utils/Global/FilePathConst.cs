using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace org.Utils.Global
{
    public static class FilePathConst
    {
        public const string EmsSolutionDirectoryName = "EmsSolutions";
        public static string EmsSolutionDirectory { get; } = Path.Combine(AppContext.BaseDirectory, EmsSolutionDirectoryName);

        public const string LogsPathName = "Logs";
        public static string LogsPath { get; } = Path.Combine(AppContext.BaseDirectory, LogsPathName);

        public const string SnapShotName = "SnapShots";
        public static string SnapShots { get; } = Path.Combine(LogsPath, SnapShotName);

        public const string GenericConfigName = "appSettings.json";
        public static string GenericConfig { get; } = Path.Combine(AppContext.BaseDirectory, "Configs", GenericConfigName);



        public const string ResourcesDirectoryName = "Resources";
        public static string ResourcesDirectory { get; } = Path.Combine(AppContext.BaseDirectory, ResourcesDirectoryName);


        public const string NewAppDirectoryName = "NewerAppFolder";
        public static string NewAppDirectory { get; } = Path.Combine(AppContext.BaseDirectory, NewAppDirectoryName);


        public const string PriviledgesDirectoryName = "Priviledges";
        public static string PriviledgesDirectory { get; } = Path.Combine(ResourcesDirectory, PriviledgesDirectoryName);
        public const string Priviledges_LoginAccountsName = "loginAccounts.json";
        public static string Priviledges_LoginAccounts { get; } = Path.Combine(PriviledgesDirectory, Priviledges_LoginAccountsName);
        public const string Priviledges_AppRolesName = "appRoles.json";
        public static string Priviledges_AppRoles { get; } = Path.Combine(PriviledgesDirectory, Priviledges_AppRolesName);
        public const string Priviledges_AccountRolesName = "accountRoles.json";
        public static string Priviledges_AccountRoles { get; } = Path.Combine(PriviledgesDirectory, Priviledges_AccountRolesName);

        public const string Resources_OthersDirectoryName = "Others";
        public static string Resources_OthersDirectory { get; } = Path.Combine(ResourcesDirectory, Resources_OthersDirectoryName);

        public const string Others_ProductModelsName = "ProductModels.json";
        public static string Others_ProductModels { get; } = Path.Combine(Resources_OthersDirectory, Others_ProductModelsName);

        public const string DeviceModule_DirectoryName = "Modules";
        public static string DeviceModule_Directory { get; } = Path.Combine(AppContext.BaseDirectory, DeviceModule_DirectoryName);

        public const string LivingTracing_DirectoryName = "LivingTraces";
        public static string LivingTracing_Directory { get; } = Path.Combine(LogsPath, LivingTracing_DirectoryName);


        public const string EmsSolution_FileExt = ".emss";
        public const string PointMapExt = ".xlsx";
        public const string RoleResourceExt = ".role.json";
        public const string LogExt = ".log";
        public const string CsvExt = ".csv";
    }
}
