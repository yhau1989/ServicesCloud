using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Infraestructure.Compras
{
    public class ValidationFilterCompras :  ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            List<ResponseError> errors = new List<ResponseError>();
            bool tieneError = false;
            ResponseError errId;
            

            string nameId = context.HttpContext.Request.Query["id_number"];
            if (nameId == null || nameId =="")
            {
                tieneError = true;
                errId = new ResponseError()
                {
                    code = "0001",
                    id =   "0001",
                    detail = "Parametro id_number requerido",
                    title = "BadRequest"
                };
                errors.Add(errId);                
            }

            string namePais = context.HttpContext.Request.Query["countryISOCode"];
            if (namePais == null || namePais == "")
            {
                tieneError = true;
                errId = new ResponseError()
                {
                    code = "0002",
                    id = "0002",
                    detail = "Parametro countryISOCode requerido",
                    title = "BadRequest"
                };
                errors.Add(errId);
            }


            if (tieneError)
            {
                var validation = new
                {
                    responseDescription = "Parametros requeridos",
                    errors = errors,
                    responseCode = "0400",
                };

                GeneralResponse response = new()
                {
                    generalResponse = validation,
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

               
            }

            base.OnResultExecuting(context);
        }
    }
}
