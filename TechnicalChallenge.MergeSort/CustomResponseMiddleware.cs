using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalChallenge.MergeSort
{
    public class CustomResponseMiddleware
    {
        #region Private Variables
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomResponseMiddleware> _logger;
        #endregion Private Variables

        #region Constructor

        /// <summary>
        /// Custom Middleware constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public CustomResponseMiddleware(RequestDelegate next, ILogger<CustomResponseMiddleware> logger)

        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// Invokes HTTP handlers and Formats the response
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;
                    await _next.Invoke(context);
                    if (!context.Response.HasStarted)
                    {
                        if (!string.IsNullOrEmpty(context.Response.ContentType))
                        {
                            if (context.Response.ContentType.Contains("application/json"))
                            {
                                var responseString = await GetResponseString(context.Response);
                                context.Response.ContentType = "application/json";
                                var formattedResponse = FormatResponse(responseString, context.Response.StatusCode, "Success");
                                context.Response.Body = originalBodyStream;
                                await UpdateHttpReponseContext(context, formattedResponse);
                            }
                            else
                            {
                                context.Response.Body.Seek(0, SeekOrigin.Begin);
                                var responseStream = new StreamReader(context.Response.Body).BaseStream;
                                context.Response.ContentLength = responseStream.Length;
                                context.Response.Body.Seek(0, SeekOrigin.Begin);
                                context.Response.Body = originalBodyStream;
                                await responseStream.CopyToAsync(context.Response.Body);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogInformation($"Exception - {ex}");
                context.Response.StatusCode = 512;
                if (!context.Response.HasStarted)
                {
                    context.Response.Body = originalBodyStream;
                    string errorMessage = ex.Message;
                    var formattedResponse = FormatErrorResponse(errorMessage, context.Response.StatusCode, "Error");
                    await UpdateHttpReponseContext(context, formattedResponse);
                }
            }
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Converts formatted response to JSON and injects into Response stream
        /// </summary>
        /// <param name="context"></param>
        /// <param name="formattedResponse"></param>
        /// <returns></returns>
        private async Task UpdateHttpReponseContext(HttpContext context, APIResponse formattedResponse)
        {
            var jsonResponse = JsonConvert.SerializeObject(formattedResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
            await context.Response.WriteAsync(jsonResponse);
        }
        
        private async Task<string> GetResponseString(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return responseText;
        }
        
        private APIResponse FormatResponse(string responseText, int statusCode, string message)
        {
            var result = JsonConvert.DeserializeObject<object>(responseText);
            var apiResponse = new APIResponse { StatusCode = statusCode, Message = message, Payload = result };
            return apiResponse;
        }

        private APIResponse FormatErrorResponse(string errorMessage, int statusCode, string message)
        {
            var apiResponse = new APIResponse { StatusCode = statusCode, Message = message, Payload = errorMessage };
            return apiResponse;
        }

        #endregion Private Methods
    }

    public static class CustomResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomResponseMiddleware>();
        }
    }
}
