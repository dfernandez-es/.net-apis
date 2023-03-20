using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace webapiautores.Middlewares
{

    public static class LoguearRespuestaHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app){
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }

    }

    public class LoguearRespuestaHTTPMiddleware
    {
        public RequestDelegate siguiente { get; }
        public ILogger<LoguearRespuestaHTTPMiddleware> logger { get; }
        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            this.logger = logger;
            this.siguiente = siguiente;
            
        }

        public async Task InvokeAsync(HttpContext contexto){
            using (var ms = new MemoryStream())
            { 
                var cuerpoOriginalRespuesta = contexto.Response.Body; 
                contexto.Response.Body = ms;
                await siguiente(contexto); 
                ms.Seek(0, SeekOrigin.Begin); 
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(cuerpoOriginalRespuesta); 
                contexto.Response.Body = cuerpoOriginalRespuesta;
                logger.LogInformation(respuesta);
            }
        }
    }
}