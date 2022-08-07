using Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// interfaz para los metodos de historico de creditos
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay Muñoz - UNICOMER
    /// fecha creación: 28/07/2022
    /// ]]>
    public interface IHistoricoCreditos
    {
        List<SalesQuotation> Get(string id_number);
    }
}
