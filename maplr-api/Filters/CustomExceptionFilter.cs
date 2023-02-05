using maplr_api.Logs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace maplr_api.Filters
{
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new
            {
                message = "Ops! Unexpected error. Please try again later."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            CustomLogs.SaveLog(context.HttpContext.Request.Method, context.Exception.Message, context.Exception.StackTrace);
            

            context.ExceptionHandled = true;
        }
    }
}
