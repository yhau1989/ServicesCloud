using Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IHistoricoCreditos
    {
        List<SalesQuotation> Get(string id_number);
    }
}
