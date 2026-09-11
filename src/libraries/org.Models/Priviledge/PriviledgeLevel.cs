using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Priviledge
{
    public enum PriviledgeLevel:byte
    {
        None = 0,
        Visible = 1,
        Executable = 2,
        Editable = 4,
        Growable = 8,
        Erasable = 16,
        All=255
    }
}
