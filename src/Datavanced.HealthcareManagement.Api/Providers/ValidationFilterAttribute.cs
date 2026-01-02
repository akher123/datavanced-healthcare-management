using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Datavanced.HealthcareManagement.Api.Providers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        private static readonly Dictionary<string, List<ParameterInfo>> CachedParameters = new Dictionary<string, List<ParameterInfo>>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Type[] types = context.ActionArguments.Select(x => x.GetType()).ToArray();

            if (types.Length == 0)
            {
                return;
            }

            string actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;

            string controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

            Type contorller = context.Controller.GetType();

            MethodInfo methodInfo = contorller.GetMethod(actionName);

            if (methodInfo == null)
            {
                return;
            }

            // get parameters from cache
            CachedParameters.TryGetValue(actionName, out List<ParameterInfo> parameters);

            if (parameters == null)
            {
                // parameters not cached yet, need to get from method
                parameters = methodInfo.GetParameters().Where(x => !x.IsOut).ToList();

                // cache parameters
                CachedParameters.Add(actionName, parameters);
            }

            Dictionary<string, object> data = new Dictionary<string, object>();

            ValidationContext validationContext = new ValidationContext(this);
            List<ValidationResult> errors = new List<ValidationResult>();

            for (int i = 0; i < parameters.Count; i++)
            {
                ParameterInfo parameter = parameters[i];

                string name = parameter.Name;
                object value = context.ActionArguments[name];

                // add parameter name and value to dictionary
                data.Add(parameters[i].Name, parameters[i]);

                // validate parameter
                validationContext.DisplayName = name;
                List<ValidationAttribute> validations = parameter.GetCustomAttributes<ValidationAttribute>().ToList();

                if (validations.Any())
                {
                    Validator.TryValidateValue(value, validationContext, errors, validations);
                }
            }

            if (errors.Any())
            {
                ErrorMessage errorMessage = new ErrorMessage { Message = string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage)) };

                if (!string.IsNullOrWhiteSpace(errorMessage.Message))
                {
                    context.Result = new BadRequestObjectResult(errorMessage);
                }
            }
        }
    }
}
