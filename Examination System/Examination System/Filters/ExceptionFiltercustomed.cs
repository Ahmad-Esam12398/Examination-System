using Microsoft.AspNetCore.Mvc.Filters;

namespace Examination_System.Filters
{
    public class ExceptionFiltercustomed:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ViewResult { ViewName = "Exception" };
                context.ExceptionHandled = true;
            }
        }
    }
}
