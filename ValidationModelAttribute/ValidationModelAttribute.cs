using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CandidateHub.ValidationModelAttribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            foreach (var key in context.ActionArguments.Keys)
            {
                if (context.ActionArguments[key] == null)
                {
                    context.Result = new BadRequestObjectResult(new { message = $"{key} cannot be null" });
                    return;
                }
            }

          
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        e.Key,
                        Errors = e.Value.Errors.Select(err => err.ErrorMessage)
                    });

                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Validation failed.",
                    Errors = errors
                });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
