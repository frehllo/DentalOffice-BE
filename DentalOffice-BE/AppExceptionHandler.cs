using DentalOffice_BE.Common.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace DentalOffice_BE
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = exception.Message
            };

            await httpContext.Response.WriteAsJsonAsync(response);
            throw new NotImplementedException();
        }
    }
}
