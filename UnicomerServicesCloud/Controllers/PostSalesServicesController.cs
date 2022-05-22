using Core.Interfaces;
using Core.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Tools;

namespace UnicomerServicesCloud.Controllers
{
    [ApiController]
    public class PostSalesServicesController : Controller
    { 

         private readonly IPostSalesServices _postSalesServices;

        private readonly IConfiguration _config;
        private IConfiguration config;
        private Logger logger = null;
        private string nameApp;

        public PostSalesServicesController(IPostSalesServices postSalesServices)
        {
            _postSalesServices = postSalesServices;

            logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            nameApp = config.GetValue<string>("nameApp");
        }
    
        //[Route("external-post-sales/service-orders"), HttpGet]
        //public ActionResult ExternalPostSales()
        //{
        //    List<int> result = new List<int>();
        //    result.Add(0);
        //    result.Add(1);
        //    result.Add(2);
        //    result.Add(3);

        //    return Ok(result);
        //}

        [Route("post-sales/service-orders"), HttpGet]
        [Route("external-post-sales/service-orders"), HttpGet]
        public ActionResult PostSales(string id_number, string countryISOCode)
        {
            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"PostSalesServicesController.PostSales, inició de llamada", nameApp, $"id_number: {id_number}, countryISOCode: {countryISOCode}", null, null, null, null, "ApiRouter");

            var response = _postSalesServices.Get(id_number);
            RootPostSalesServices rootPostSalesServices = new RootPostSalesServices()
            {
                serviceOrder = response,
            };

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"PostSalesServicesController.PostSales, fin de llamada", nameApp, $"id_number: {id_number}, countryISOCode: {countryISOCode}", $"response: {JsonConvert.SerializeObject(rootPostSalesServices)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, "ApiRouter");

            return Ok(rootPostSalesServices);
        }
    }
}

