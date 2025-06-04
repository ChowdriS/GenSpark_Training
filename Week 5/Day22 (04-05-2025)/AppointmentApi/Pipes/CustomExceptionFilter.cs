using System;
using AppointmentApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppointmentApi.Pipes;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new BadRequestObjectResult(new CustomExceptionDTO
        {
            errorNumber = 500,
            errorMessage = context.Exception.Message
        });
    }
}
