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
    public class HistoricoCreditoController : Controller
    {
        private readonly IHistoricoCreditos _HistoricoCreditos;
        private readonly IConfiguration _config;
        private IConfiguration config;
        private Logger logger = null;
        private string nameApp;


        /// <summary>
        /// Construnctor
        /// </summary>
        /// <param name="configuration">Objeto interfaz</param>
        /// <param name="historicoCreditos">Objeto interfaz</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public HistoricoCreditoController(IConfiguration configuration, IHistoricoCreditos historicoCreditos)
        {
            _config = configuration;
            _HistoricoCreditos = historicoCreditos;

            logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            nameApp = config.GetValue<string>("nameApp");
        }



        /// <summary>
        /// metodo de entrada del endpoint
        /// </summary>
        /// <param name="id_type">tipo de identificacion</param>
        /// <param name="id_number">ci del cliente</param>
        /// <param name="get_history">indicador de listar historico</param>
        /// <param name="countryISOCode">codigo del pais</param>
        /// <returns></returns>
        [Route("external-credits/sales-quotations"), HttpGet]
        [Route("credits/sales-quotations"), HttpGet]
        public ActionResult Credits(string id_type, string id_number, string get_history, string countryISOCode)
        {
            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"HistoricoCreditoController.Credits, inició de llamada", nameApp, $"id_type: {id_type}, id_number: {id_number}, get_history: {get_history}, countryISOCode: {countryISOCode}", null, null, null, null, "ApiRouter");

            var response = _HistoricoCreditos.Get(id_number);
            RootHistoricoCreditoResponse historicoCreditos = new RootHistoricoCreditoResponse()
            {
                salesQuotation = response,
            };

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"HistoricoCreditoController.Credits, fin de llamada", nameApp, $"id_type: {id_type}, id_number: {id_number}, get_history: {get_history}, countryISOCode: {countryISOCode}", $"response: {JsonConvert.SerializeObject(historicoCreditos)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, "ApiRouter");

            return Ok(historicoCreditos);
        }
    }
}
