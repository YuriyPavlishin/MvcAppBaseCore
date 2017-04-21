using System.Xml.Serialization;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    public class ApiResult
    {
        public enum ApiResultCodes
        {
            Success = 0,
            Exception = 1,
            EntityNotFound = 2,
            ArgumentValidationFailed = 3,
            FakeException = 99
        }

        public int ResultCode { get; set; }

        public ApiResultCodes ResultCodeName
        {
            get { return (ApiResultCodes)ResultCode; }
            set { ResultCode = (int)value; }
        }

        public string ErrorMessage { get; set; }

        public static ApiResult Error(ApiResultCodes resultCode, string errorMessage = "")
        {
            return new ApiResult()
            {
                ResultCodeName = resultCode,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResult Success()
        {
            return new ApiResult();
        }

        public static ApiResult<T> Success<T>(T result)
        {
            return new ApiResult<T> { ReturnValue = result };
        }

        public static ApiResult<T> Error<T>(ApiResultCodes resultCode, string errorMessage = "")
        {
            return new ApiResult<T> { ResultCodeName = resultCode, ErrorMessage = errorMessage };
        }

        public ApiResult()
        {
            ResultCodeName = ApiResultCodes.Success;
            ErrorMessage = string.Empty;
        }
    }

    [XmlRoot("ApiResult")]
    public class ApiResult<T> : ApiResult
    {
        public T ReturnValue { get; set; }
    }
}
