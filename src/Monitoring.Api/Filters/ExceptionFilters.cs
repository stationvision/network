using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Monitoring.Api.Filters
{
    public class ExceptionFilters : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilters> _logger;

        public ExceptionFilters(ILogger<ExceptionFilters> logger)
        {
            _logger = logger;
        }

        //TODO: create custom exceptions
        //Remove logging of exceptions from code, handle them here instead

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = HttpStatusCode.InternalServerError;
            var exceptionType = exception.GetType();
            if (exceptionType.Name is nameof(ArgumentException))
                statusCode = HttpStatusCode.BadRequest;
            else if (exceptionType.Name is nameof(NotImplementedException))
                statusCode = HttpStatusCode.NotImplemented;
            else if (exceptionType.Name is nameof(UnauthorizedAccessException))
                statusCode = HttpStatusCode.Unauthorized;
            else if (exceptionType.Name is nameof(FileNotFoundException))
                statusCode = HttpStatusCode.NotFound;
            else if (exceptionType.Name is nameof(TimeoutException))
                statusCode = HttpStatusCode.RequestTimeout;
            else if (exceptionType.Name is nameof(ObjectDisposedException))
                statusCode = HttpStatusCode.Gone;
            else if (exceptionType.Name is nameof(NotSupportedException))
                statusCode = HttpStatusCode.NotAcceptable;
            else if (exceptionType.Name is nameof(NullReferenceException))
                statusCode = HttpStatusCode.BadRequest;


            var errors = new List<string> { exception.Message };
            if (exception.InnerException != null)
            {
                errors.Add(exception.InnerException.Message);
            }

            var resultModel = new ExceptionResultModel()
            {
                Errors = errors.ToArray(),
                Status = (int)statusCode,
                Title = $"{exception.Source} - {exception.Message}",
            };

            var result = new ObjectResult(resultModel)
            {
                StatusCode = (int)statusCode
            };

            _logger.LogError(exception, $"ExceptionFilters: Error in {context.ActionDescriptor.DisplayName}. {exception.Message}. Stack Trace: {exception.StackTrace}");

            context.Result = result;

            context.ExceptionHandled = true;

        }

        public class ExceptionResultModel
        {
            public string Title { get; set; }
            public int Status { get; set; }
            public string[] Errors { get; set; }
        }

    }
}
