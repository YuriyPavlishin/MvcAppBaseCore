using System;
using System.Collections.Generic;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    public class ApiException : Exception
    {
        public ApiResult.ApiResultCodes ErrorCode { get; private set; }

        public ApiException(string message, Exception innerException = null)
            : this(ApiResult.ApiResultCodes.Exception, message, innerException)
        {
        }

        public ApiException(ApiResult.ApiResultCodes errorCode, string message = null, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return $"Api error code: {ErrorCode}{Environment.NewLine}{base.ToString()}";
        }
        
        public static string GetArgumentValidationFailedMessage(List<string> errorsList)
        {
            for (var i = 0; i < errorsList.Count; i++)
            {
                var str = errorsList[i].Trim();
                if (!string.IsNullOrWhiteSpace(str) && !str.EndsWith(".") && !str.EndsWith(";"))
                {
                    errorsList[i] = str + ".";
                }
            }

            return string.Join(" ", errorsList);
        }
    }
}
