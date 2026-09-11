using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Athena
{
    public class AppReleaseNote
    {
        public int MajorVersion { get; set; }

        public int MinorVersion { get; set; }

        public int BuildVersion { get; set; }

        public string Revision { get; set; }

        public string AppName { get; set; }

        public string Asset { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
