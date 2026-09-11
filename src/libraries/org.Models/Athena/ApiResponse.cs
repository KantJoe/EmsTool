using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Athena
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }
}
