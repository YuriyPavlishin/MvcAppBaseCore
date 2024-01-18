using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiInvalidModelStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsList = new List<string>();

                foreach (var field in context.ModelState)
                {
                    if (field.Value.Errors.Count > 0)
                    {
                        string[] propNames = field.Key.Split('.');
                        if (propNames.Length > 1)
                        {
                            var actionParameteres = context.ActionDescriptor.Parameters.Select(x => x.Name);
                            if (actionParameteres.Any(m => String.Equals(propNames[0], m, StringComparison.OrdinalIgnoreCase)))
                            {
                                //skip action method parameter name
                                propNames = propNames.Skip(1).ToArray();
                            }
                        }

                        string fName = string.Join(".", propNames);
                        if (!String.IsNullOrWhiteSpace(fName))
                        {
                            fName += ": ";
                        }

                        errorsList.AddRange(field.Value.Errors
                            .Select(m => fName + (NullIfEmpty(m.ErrorMessage) ?? (m.Exception != null ? m.Exception.Message : ""))));
                    }
                }

                throw new ApiException(ApiResult.ApiResultCodes.ArgumentValidationFailed, ApiException.GetArgumentValidationFailedMessage(errorsList));
            }
        }

        private string NullIfEmpty(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }
    }
}
