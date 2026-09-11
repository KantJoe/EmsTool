using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace org.Utils.Global
{
    public class ModuleContext
    {
        public static ModuleContext Instance { get; private set; }

        public Dictionary<string, IModule> ModulePool { get; private set; }

        private ModuleContext()
        {
            ModulePool = new Dictionary<string, IModule>();
        }

        static ModuleContext()
        {
            Instance = new ModuleContext();
        }

        public static void Initialize()
        {

            if (!Directory.Exists(FilePathConst.DeviceModule_Directory))
            {
                Directory.CreateDirectory(FilePathConst.DeviceModule_Directory);
            }
        }

        public IModule GetModule(string key)
        {
            if (ModulePool.TryGetValue(key, out var module))
            {
                return module;
            }

            return default;
        }
    }
}
