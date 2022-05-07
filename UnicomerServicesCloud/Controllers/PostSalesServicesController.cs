using Core.Interfaces;
using Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace UnicomerServicesCloud.Controllers
{
    [ApiController]
    public class PostSalesServicesController : Controller
    { 

         private readonly IPostSalesServices _postSalesServices;

        public PostSalesServicesController(IPostSalesServices postSalesServices)
        {
            _postSalesServices = postSalesServices;
        }
    
        [Route("external-post-sales/service-orders"), HttpGet]
        public ActionResult ExternalPostSales()
        {
            List<int> result = new List<int>();
            result.Add(0);
            result.Add(1);
            result.Add(2);
            result.Add(3);

            return Ok(result);
        }

        [Route("post-sales/service-orders"), HttpGet]
        public ActionResult PostSales(string id_number, string countryISOCode)
        {
            var response = _postSalesServices.Get(id_number);
            RootPostSalesServices rootPostSalesServices = new RootPostSalesServices()
            {
                serviceOrder = response,
            };
            return Ok(rootPostSalesServices);
        }
    }
}

