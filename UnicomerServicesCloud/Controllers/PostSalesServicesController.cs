using Microsoft.AspNetCore.Mvc;

namespace UnicomerServicesCloud.Controllers
{
    [ApiController]
    public class PostSalesServicesController : Controller
    {
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
        public ActionResult PostSales()
        {
            List<int> result = new List<int>();
            result.Add(0);
            result.Add(1);
            result.Add(2);
            result.Add(3);

            return Ok(result);
        }
    }
}
