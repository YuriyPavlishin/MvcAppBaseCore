using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BaseApp.Web.Code.Infrastructure.Validation;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    public class ApiResult
    {
        public static implicit operator ApiResult(ValidatedValue validatedValue) => Success(validatedValue);
        
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
        
        public static ApiResult Success(ValidatedValue validatedValue)
        {
            validatedValue.Ensure();
            return Success();
        }
        
        public static ApiResult<T> Success<T>(ValidatedValue<T> validatedValue)
        {
            return Success(validatedValue.EnsuredValue);
        }

        public static ApiResult<T> Success<T>(T result)
        {
            return new ApiResult<T> { ReturnValue = result };
        }
        
        public static ApiResultList<T> SuccessListWrap<T>(List<T> resultList)
        {
            return new ApiResultList<T> {ReturnValue = new ApiResultListWrap<T>() {Items = resultList } };
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
        public static implicit operator ApiResult<T>(T result) => Success(result);
        public static implicit operator ApiResult<T>(ValidatedValue<T> validatedValue) => Success(validatedValue);
    }
    
    [XmlRoot("ApiResult")]
    public class ApiResultList<T> : ApiResult<ApiResultListWrap<T>>
    {
        public static implicit operator ApiResultList<T>(List<T> result) => SuccessListWrap(result);
    }
    
    public class ApiResultListWrap<T>
    {
        public List<T> Items { get; set; }
    }
}
