using org.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace org
{
    public interface IModule
    {
        string Key { get; }
        void Load();

        void Unload();

        EmsSettings ResetEmsSetting();
    }
}
