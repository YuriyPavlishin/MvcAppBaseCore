using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Common.Extensions;
using BaseApp.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    public class ApiControllerConvention:IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsSubclassOf(typeof(ControllerBaseApi)))
                return;

            foreach (var action in controller.Actions)
            {
                ApplyActionConvention(action);
            }
        }

        public void ApplyActionConvention(ActionModel action)
        {
            if (action.Attributes.Count == 0 || action.Attributes.OfType<HttpGetAttribute>().Any())
                return;
            if (action.Parameters.Count != 1)
                return;

            var parameter = action.Parameters.Single();
            if (parameter.BindingInfo?.BindingSource != null ||
                parameter.Attributes.OfType<IBindingSourceMetadata>().Any())
                return;

            parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
            var bindingSource = parameter.ParameterInfo.ParameterType.IsAssignableFromOrContainsType<IFormFile>() 
                ? BindingSource.Form : BindingSource.Body;
            parameter.BindingInfo.BindingSource = bindingSource;
        }
    }
}
