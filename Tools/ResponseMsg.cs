using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// Clase base
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay - UNICOMER
    /// fecha creación: 19-07-022
    /// ]]>
    public class ResponseMsg
    {
        public int status { get; set; }
        public string? msg { get; set; }
        public object? data { get; set; }
    }
}
