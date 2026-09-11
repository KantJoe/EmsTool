using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class EmsSolutionDocument
    {
        public DateTime? CreatedDatetime { get; set; }

        public DateTime? LastUpdatedDatetime { get; set; }

        public string Creater { get; set; }

        public string LastUpdateEditor { get; set; }

        public string SoftwareVersionOfCreated { get; set; }
    }
}
