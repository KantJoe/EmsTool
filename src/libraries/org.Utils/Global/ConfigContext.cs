using org.Models;
using org.Models.Athena;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace org.Utils.Global
{
    public class ConfigContext
    {
        private static ReaderWriterLockSlim _lockSlim;
        public static GenericConfig GenericConfig { get; private set; }
        public static AppReleaseNote NewestAppVersion { get; set; }

        public static ProductModelCategory ModelCategory { get; private set; }
        static ConfigContext()
        {
            _lockSlim = new ReaderWriterLockSlim();
        }

        public static void Initialize()
        {
            LoadGeneriConfig();
            LoadProductModels();
        }

        private static void LoadProductModels()
        {
            if (!File.Exists(FilePathConst.Others_ProductModels))
            {
                throw new FileNotFoundException(FilePathConst.Others_ProductModels + " not found");
            }

            var json = File.ReadAllText(FilePathConst.Others_ProductModels);
            ModelCategory = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductModelCategory>(json);
        }

        private static void LoadGeneriConfig()
        {
            if (!File.Exists(FilePathConst.GenericConfig))
            {
                throw new FileNotFoundException(FilePathConst.GenericConfig + " not found");
            }

            var json = File.ReadAllText(FilePathConst.GenericConfig);
            GenericConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericConfig>(json);
        }

        public static async Task SaveConfigAsync()
        {
            _lockSlim.EnterWriteLock();
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(GenericConfig,Newtonsoft.Json.Formatting.Indented);
                await File.WriteAllTextAsync(FilePathConst.GenericConfig, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "ConfigContext.SaveConfig");
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }
    }
}
