using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace org.Utils.Global
{
    public class AssemblyContext
    {
        public static AssemblyContext Instance { get; private set; }

        private AssemblyContext()
        {
            AssemblyPool = new Dictionary<string, Assembly>();
        }

        static AssemblyContext()
        {
            Instance = new AssemblyContext();
        }

        public Dictionary<string, Assembly> AssemblyPool { get; private set; }

        public bool LoadAssembley(string key)
        {
            if (AssemblyPool.ContainsKey(key))
            {
                return true;
            }

            var filePath = Path.Combine(AppContext.BaseDirectory, "Modules", $"org.{key}.dll");
            if (!File.Exists(filePath))
            {
                return false;
            }

            var buffer = File.ReadAllBytes(filePath);

            var assembly = Assembly.Load(buffer);
            AssemblyPool[key] = assembly;
            var module = (IModule)assembly.CreateInstance($"org.{key}.{key}Module");
            module.Load();
            return true;
        }
    }
}
