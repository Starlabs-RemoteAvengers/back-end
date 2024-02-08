using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
