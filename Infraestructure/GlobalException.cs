using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.Odbc;
using System.Diagnostics;
using System.Net;

namespace Infraestructure
{
    public class GlobalException : IExceptionFilter
    {

        public void OnException (ExceptionContext context)
        {
            
            //para controlar excepciones de base de datos 
            if (context.Exception.GetType() == typeof(OdbcException))  
            {
                var exception = (OdbcException)context.Exception;
                var validation = new
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Details = exception.Message
                    

                };

               

                context.Result = new ObjectResult(validation)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }           
           
            // para controlar excepciones de cualquier otro tipo 
            else
            {
                var exception = (System.Exception)context.Exception;
                var validation = new
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Details = exception.GetType().Name  + ' ' +   exception.Message
                };                

                context.Result = new ObjectResult(validation)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }   
}
