using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Datavanced.HealthcareManagement.Api.Providers
{
    /// <summary>
    /// Action filter to check the model state before the controller action is invoked
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (ModelErrorCollection modelStateVal in context.ModelState.Values.Select(d => d.Errors))
                {
                    errors.AddRange(modelStateVal.Select(error => error.ErrorMessage));
                }
                ResponseMessage<bool> errorMessage =
                    new ResponseMessage<bool> { Message = string.Join("<br>", errors.Select(x => x)) };
                if (!string.IsNullOrWhiteSpace(errorMessage.Message))
                {
                    context.Result = new BadRequestObjectResult(errorMessage);
                }
            }
        }
    }
}
