using a_no_da.xtools.core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace a_no_da.xtools.core.Attributes
{
    public class XToolsExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var xtoolsException = context.Exception as XToolsException;

            if (xtoolsException != null)
            {
                context.Result = new ObjectResult(xtoolsException.Message)
                {
                    StatusCode = 500
                };

                return;
            }

            var xtoolsValidationException = context.Exception as XToolsValidationException;

            if (xtoolsValidationException != null)
            {
                return;
            }

            context.Result = new ObjectResult("Internal Server Error")
            {
                StatusCode = 500
            };
        }
    }
}
