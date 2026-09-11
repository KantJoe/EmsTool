using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute(string column)
        {
            Column = column;
        }

        public string Column { get; set; }
    }
}
