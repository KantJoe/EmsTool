using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace org.Utils
{
    public class EmptyFileLifecycleHooks : FileLifecycleHooks
    {

        public override void OnFileDeleting(string path)
        {
            Debug.WriteLine($"LogFile Deleting! {path}");
        }
    }
}
