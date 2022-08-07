using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;


namespace Infraestructure
{
    /// <summary>
    /// Clase que intersecta los request y los valida
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay Muñoz
    /// fecha creación: 28/07/2022
    /// ]]>
    public class ValidationFilter :  ActionFilterAttribute
    {
        /// <summary>
        /// Implementación para personalizar los response en de estados de error 
        /// </summary>
        /// <param name="context">contexto del request interceptado</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            List<ResponseError> errors = new List<ResponseError>();
            bool tieneError = false;
            ResponseError errId;

            string pathPostSalesOrder = "/post-sales/service-orders";
            string pathHistCompras = "/storeops/retail-transactions";


            // validaciones ruta pathHistCompras
            string nameIdCompra = context.HttpContext.Request.Query["id_number"];
            if ((nameIdCompra == null || nameIdCompra == "") && context.HttpContext.Request.Path.Value == pathHistCompras)
            {
                tieneError = true;
                errId = new ResponseError()
                {
                    code = "0001",
                    id =   "0001",
                    detail = "Parametro id_number requerido retail-transactions",
                    title = "BadRequest"
                };
                errors.Add(errId);                
            }
            string namePaisCompra = context.HttpContext.Request.Query["countryISOCode"];
            if ((namePaisCompra == null || namePaisCompra == "") && context.HttpContext.Request.Path.Value == pathHistCompras)
            {
                tieneError = true;
                errId = new ResponseError()
                {
                    code = "0002",
                    id = "0002",
                    detail = "Parametro countryISOCode requerido retail-transactions",
                    title = "BadRequest"
                };
                errors.Add(errId);
            }


            // validaciones ruta pathPostSalesOrder

            string nameIdPostSales = context.HttpContext.Request.Query["id_number"];
            if ((nameIdPostSales == null || nameIdPostSales == "") && context.HttpContext.Request.Path.Value == pathPostSalesOrder)
            {
                tieneError = true;
                errId = new ResponseError()
                {
                    code = "0001",
                    id = "0001",
                    detail = "Parametro id_number requerido PostSalesOrder",
                    title = "BadRequest"
                };
                errors.Add(errId);
            }
            string namePaisPostSales = context.HttpContext.Request.Query["countryISOCode"];
            if ((namePaisPostSales == null || namePaisPostSales == "") && context.HttpContext.Request.Path.Value == pathPostSalesOrder)
            {
                tieneError = true;
                errId = new ResponseError()
                {
                    code = "0002",
                    id = "0002",
                    detail = "Parametro countryISOCode requerido PostSalesOrder",
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
