using Microsoft.AspNetCore.Mvc;

namespace UnicomerServicesCloud.Controllers
{
    public class HistoricoCreditoController : Controller
    {
        private readonly IConfiguration _config;
        private IConfiguration config;
        //private Logger logger = null;
        private string nameApp;

        //public HistoricoCreditoController(IConfiguration configuration, IHistoricoCompras historicoCompras)
        public HistoricoCreditoController(IConfiguration configuration)
        {
            _config = configuration;
            //_HistoricoCompras = historicoCompras;

            //logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            nameApp = config.GetValue<string>("nameApp");
        }
    }
}
