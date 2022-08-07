using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.Odbc;
using System.Diagnostics;
using System.Net;

namespace Infraestructure
{
    /// <summary>
    /// Clase para menejar errores de forma global en todos los endpoints
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay Muñoz - UNICOMER
    /// fecha creación: 28/07/2022
    /// ]]>
    public class GlobalException : IExceptionFilter
    {
        /// <summary>
        /// Retorna el detalle de las exepciones de los errores
        /// </summary>
        /// <param name="context">Contexto del error capturado</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        public void OnException (ExceptionContext context)
        {

            List<ResponseError> errors = new List<ResponseError>();

            //para controlar excepciones de base de datos 
            if (context.Exception.GetType() == typeof(OdbcException))  
            {

                var exception = (OdbcException)context.Exception;
                ResponseError Error = new ResponseError()
                {
                    code = "0500",
                    id = exception.ErrorCode.ToString(),
                    detail = exception.Message,
                    title = "Internal Server Error"
                };
                errors.Add(Error);

                                
                var validation = new
                {
                    responseDescription = exception.Message,
                    errors = errors,                   
                    responseCode = "0500",
                };

                GeneralResponse response = new()
                {
                    generalResponse = validation,
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }           
           
            // para controlar excepciones de cualquier otro tipo 
            else
            {
                var exception = (System.Exception)context.Exception;
                ResponseError Error = new ResponseError()
                {
                    code = "0500",
                    id = exception.GetType().Name,
                    detail = exception.Message,
                    title = "Internal Server Error"
                };
                errors.Add(Error);
                var validation = new
                {

                    responseDescription = exception.GetType().Name + ' ' + exception.Message,
                    errors = errors,
                    responseCode = "0500"                   
                };

                GeneralResponse response = new()
                {
                    generalResponse = validation,
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

       

       
    }   
}
