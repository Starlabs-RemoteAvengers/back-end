using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppointEase.Application.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AppointEase.Application.Filters.ValidationException validationException)
            {
                context.Result = new BadRequestObjectResult(validationException.Errors);
                context.ExceptionHandled = true;
            }
        }

    }
}
