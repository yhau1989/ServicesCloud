using Core.Interfaces;
using Core.Response;
using Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace UnicomerServicesCloud.Controllers
{
    [ApiController]
    public class HistoricoCompraController : Controller
    {
        private readonly IHistoricoCompras _HistoricoCompras;

        public HistoricoCompraController(IHistoricoCompras historicoCompras)
        {
            _HistoricoCompras = historicoCompras;
        }
    
        [Route("external-storeops/retail-transactions"), HttpGet]
        public ActionResult ExternalStoreops()
        {
            List<int> result = new List<int>();
            result.Add(0);
            result.Add(1);
            result.Add(2);
            result.Add(3);

            return Ok(result);
        }

        [Route("storeops/retail-transactions"), HttpGet]
        [ServiceFilter(typeof(ValidationFilter))]
        public ActionResult Storeops(string id_number, string countryISOCode)       
        {
            var response = _HistoricoCompras.Get(id_number);
            RootRetailTransaction retailTransaction = new RootRetailTransaction()
            {
                retailTransaction = response,
            };
            return Ok(retailTransaction);
        }
    }
}
