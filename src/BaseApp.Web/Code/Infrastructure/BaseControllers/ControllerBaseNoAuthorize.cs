using System;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Common.Extensions;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseApp.Web.Code.Infrastructure.BaseControllers
{
    public class ControllerBaseNoAuthorize: ControllerSiteBase
    {
        /// <summary>
        /// client messages holder. ClientMessage.Render(ViewContext) call required in MasterPage 
        /// (or in current view  if ajax update used)
        /// </summary>
        protected ScriptMessage ClientMessage { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ClientMessage = new ScriptMessage();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var httpResponse = context.HttpContext.Response;
            ClientMessage.SaveMessages(httpResponse.Cookies);

            var redirectResult = (context.Result is RedirectToActionResult || context.Result is RedirectResult);
            if (redirectResult && context.HttpContext.Request.IsAjaxRequest())
            {
                httpResponse.OnStarting(() =>
                    {
                        //if we clear response than we lose all our headers (location, coockies, and other)
                        //httpResponse.Clear();
                        //httpResponse.WriteAsync(new string(' ', 512));//.Write(new string(' ', 512)); //fix opera issue
                        httpResponse.Headers.Add("AjaxRedirect", "1");
                        httpResponse.StatusCode = 200;
                        
                        return Task.FromResult(0);
                    });

            }

            base.OnActionExecuted(context);
        }

        protected IActionResult NotFoundAction()
        {
            return NotFound();
        }
        
        protected async Task<bool> ValidateAndPerform(Func<Task<ValidatedValue>> perform)
        {
            if (!ModelState.IsValid)
                return false;
            var validateResult = await perform();
            if (!validateResult.IsValid)
            {
                AddToModelState(validateResult);
                return false;
            }
            return true;
        }
        
        protected async Task<(bool isValid, T result)> ValidateAndPerform<T>(Func<Task<ValidatedValue<T>>> perform)
        {
            if (!ModelState.IsValid)
                return (false, default);
            var validateResult = await perform();
            if (!validateResult.IsValid)
            {
                AddToModelState(validateResult);
                return (false, default);
            }
            return (true, validateResult.EnsuredValue);
        }

        private void AddToModelState(ValidatedValue validateResult)
        {
            var commonErrorMessage = "; ".UseForJoinNonEmpty(validateResult.ValidationItems.Where(x => string.IsNullOrEmpty(x.PropertyName)).Select(x => x.ErrorMessage));
            if (!string.IsNullOrWhiteSpace(commonErrorMessage))
            {
                ModelState.AddModelError("", commonErrorMessage);
            }

            foreach (var item in validateResult.ValidationItems.Where(x => !string.IsNullOrEmpty(x.PropertyName)))
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }
        }

        protected ContentResult CloseDialog(CloseDialogArgs args = null)
        {
            Response.Headers.Add("CloseDialog", "1");

            args = args ?? CloseDialogArgs.GetDefault();

            if (!string.IsNullOrWhiteSpace(args.ReturnResult))
            {
                Response.Headers.Add("CloseDialogResult", args.ReturnResult);
            }
            if (!args.DoNotShowSuccessMessage)
            {
                var msg = args.SuccessMessage;
                if (string.IsNullOrWhiteSpace(msg))
                    msg = "Saved successfully";

                ClientMessage.AddSuccess(msg);
            }

            return Content("");
        }

        public class CloseDialogArgs
        {
            public string SuccessMessage { get; set; }
            public string ReturnResult { get; set; }
            public bool DoNotShowSuccessMessage { get; set; }

            public static CloseDialogArgs GetDefault()
            {
                return new CloseDialogArgs();
            }
        }
    }
}
