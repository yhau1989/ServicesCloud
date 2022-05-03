﻿using Microsoft.AspNetCore.Http;
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

            List<err> errors = new List<err>();

            //para controlar excepciones de base de datos 
            if (context.Exception.GetType() == typeof(OdbcException))  
            {

                var exception = (OdbcException)context.Exception;

               
                err Error = new err()
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

                
                err Error = new err()
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

        private class err
        {
            public string code { get; set; }
            public string id { get; set; }
            public string detail { get; set; }
            public string title  { get; set; }

        }

        private class GeneralResponse
        {
            public object generalResponse { get; set; }
            

        }
    }   
}
