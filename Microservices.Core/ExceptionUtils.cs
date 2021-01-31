using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

namespace Microservices.Core
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    await HandleExceptionAsync(context, ex);
                }
                else
                {
                    throw;
                }

            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as BaseApiException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Unexpected error";
            var description = "Unexpected error";

            if (null != customException)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = description;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = message,
                Description = description
            }));
        }
    }

    public class ApiResult
    {
        public bool Succeeded { get; set; }
        public BaseApiException Exception { get; set; }

        public ApiResult(bool succeeded, Exception exception = null)
        {
            Succeeded = succeeded;
            if (exception != null)
            {
                Exception = JsonConvert.DeserializeObject<BaseApiException>(JsonConvert.SerializeObject(exception));
            }
        }

        public static ApiResult Success()
        {
            return new ApiResult(true);
        }
        public static ApiResult Failed(Exception ex)
        {
            return new ApiResult(false,ex);
        }


    }
    public class BaseApiException : Exception
    {
        public int Code { get; }
        public string Description { get; }
        public BaseApiException() : base("Error")
        {
            Code = 500;
            Description = "Error";
        }
        public BaseApiException(string message, string description, int code) : base(message)
        {
            Code = code;
            Description = description;
        }
    }
    public class ApiException : BaseApiException
    {
        public ApiException()
        {
        }
        public ApiException(HttpStatusCode statusCode, string description) 
            : base($"{statusCode}: {description}", description, (int)statusCode)
        {
        }

        public ApiException(string statusCode, string description) 
            : base($"{statusCode} {description}", description, 
                int.TryParse(statusCode, out int parsed)? parsed : 500)
        {
        }
    }
    public class CustomErrorResponse
    {
        public string Message { get; set; }
        public string Description { get; set; }
    }
    public static class CustomExceptionMiddlewareExtensions

    {

        public static IApplicationBuilder UseApiExceptionMiddleware(this IApplicationBuilder builder)

        {

            return builder.UseMiddleware<ApiExceptionMiddleware>();

        }

    }
}