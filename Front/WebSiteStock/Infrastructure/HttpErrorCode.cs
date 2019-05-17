namespace SAC.Stock.Front.Infrastructure
{
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Seed.Validator;    
    using System.Collections.Generic;    
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;

    internal static class HttpErrorCode
    {
        private static HttpErrorData @default;

        private static HttpErrorData failedConnectDatabase;

        public static HttpErrorData Default
        {
            get
            {
                return @default
                       ?? (@default =
                           new HttpErrorData
                           {
                               Code = "Default",
                               StatusCode = HttpStatusCode.InternalServerError,
                               Message =
                                 "Se presento una condición de falla. Por favor, reintente y si se repite el inconveniente comuníquese con el soporte técnico del sistema."
                           });
            }
        }

        public static HttpErrorData FailedConnectDatabase
        {
            get
            {
                return failedConnectDatabase
                       ?? (failedConnectDatabase =
                           new HttpErrorData
                           {
                               Code = "FailedConnectDatabase",
                               StatusCode = HttpStatusCode.InternalServerError,
                               Message =
                                 "Falló la conexión con la Base de datos."
                           });
            }
        }

        public static HttpErrorData OptionNoAuthorized
        {
            get
            {
                return failedConnectDatabase
                       ?? (failedConnectDatabase =
                           new HttpErrorData
                           {
                               Code = "OptionNoAuthorized",
                               StatusCode = HttpStatusCode.InternalServerError,
                               Message =
                               "Opción no autorizada para el usuario actual."
                           });
            }
        }

        public static HttpResponseMessage FromServiceException(DistributedServiceException exception, HttpRequestMessage request)
        {
            var error = new HttpErrorData
            {
                Code = exception.Code,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = exception.Message
            };
            return error.CreateErrorResponse(request);
        }

        public static HttpResponseMessage ErrorDataFromReport(IEnumerable<InvalidItem> report, HttpRequestMessage request, string[] codes)
        {                     
            var first = report.FirstOrDefault(r => codes.Contains(r.Code));
            return first != null
                     ? (new HttpErrorData { Code = first.Code, StatusCode = HttpStatusCode.InternalServerError })
                         .CreateErrorResponse(first.Message, request)
                     : null;                        
        }
      
        internal class HttpErrorData
        {

            public HttpStatusCode StatusCode { get; set; }

            public string Code { get; set; }

            public string Message { get; set; }

            public HttpResponseMessage CreateErrorResponse(HttpRequestMessage request, params object[] args)
            {
                return this.CreateErrorResponse(string.Format(this.Message, args), request);
            }

            public HttpResponseMessage CreateErrorResponse(string message, HttpRequestMessage request)
            {
                var error = new HttpError(message.Replace("\n", "<br />"));
                error["HttpErrorCode"] = this.Code;
                return request.CreateErrorResponse(this.StatusCode, error);
            }
        }        
    }
}