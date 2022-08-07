using Core.Interfaces;
using Core.Response;
using Infraestructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Tools;

namespace UnicomerServicesCloud.Controllers
{
    [ApiController]
    public class HistoricoCompraController : Controller
    {
        private readonly IHistoricoCompras _HistoricoCompras;

        private readonly IConfiguration _config;
        private IConfiguration config;
        private Logger logger = null;
        private string nameApp;


        /// <summary>
        /// Construnctor
        /// </summary>
        /// <param name="configuration">Objeto interfaz</param>
        /// <param name="historicoCompras">Objeto interfaz</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public HistoricoCompraController(IConfiguration configuration, IHistoricoCompras historicoCompras)
        {
            _config = configuration;
            _HistoricoCompras = historicoCompras;

            logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            nameApp = config.GetValue<string>("nameApp");
        }
    
        

        /// <summary>
        /// metodo de entrada del endpoint 
        /// </summary>
        /// <param name="id_number">ci del cliente</param>
        /// <param name="countryISOCode">codigo de pais</param>
        /// <returns></returns>
        [Route("external-storeops/retail-transactions"), HttpGet]
        [Route("storeops/retail-transactions"), HttpGet]
        public ActionResult Storeops(string id_number, string countryISOCode)       
        {
            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"HistoricoCompraController.Storeops, inició de llamada", nameApp, $"id_number: {id_number}, countryISOCode: {countryISOCode}", null, null, null, null, "ApiRouter");

            var response = _HistoricoCompras.Get(id_number);
            RootRetailTransaction retailTransaction = new RootRetailTransaction()
            {
                retailTransaction = response,
            };

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"HistoricoCompraController.Storeops, fin de llamada", nameApp, $"id_number: {id_number}, countryISOCode: {countryISOCode}", $"response: {JsonConvert.SerializeObject(retailTransaction)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, "ApiRouter");

            return Ok(retailTransaction);
        }
    }
}
