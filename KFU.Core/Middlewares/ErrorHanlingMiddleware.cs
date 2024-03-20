using KFU.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace Portal.Ui.Middlewares
{
    public class ErrorHanlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHanlingMiddleware> _logger;
        private string ErrorMessage {  get; set; } = string.Empty;
        public ErrorHanlingMiddleware(ILogger<ErrorHanlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }
            catch (Exception ex)
            {       
                _logger.LogError(ex , "An Unexpected Error ");
                await HandleExceptionAsync(context, ex);                          
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject( new DefaultResponse() { ErrorMessage= "حدث خطأ غير متوقع ، حاول لاحقا", Status = 500 , Success = false}));
        }
       
    }
}
