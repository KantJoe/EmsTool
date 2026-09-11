using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class MultiLangConfig
    {
        public List<MultiLangResource> Supports { get; set; }

        public string DefaultCode { get; set; }
    }
}
