using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    //objeto para todas las transacciones de base de datos
    public class ResponseMsg
    {
        public int status { get; set; }
        public string? msg { get; set; }
        public object? data { get; set; }
    }
}
