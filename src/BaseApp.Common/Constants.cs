namespace BaseApp.Common
{
    public class Constants
    {
        public const int PageSizeDefault = 14;
        public const int CountryUSA_Id = 223;
        public const int MinPasswordLength = 7;

        public class Roles
        {
            public const string Admin = "admin";
            public const string User = "user";
        }
        
        public class RegularExpressions
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format?redirectedfrom=MSDN
            public const string Email =
                @"^((([-0-9_a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~a-zA-Z_0-9])*)([-0-9_a-zA-Z])@)|([-0-9_a-zA-Z]+@))"
                + @"((([-0-9_a-zA-Z][-0-9_a-zA-Z]*[-0-9_a-zA-Z]*\.)+[-a-zA-Z_0-9][-a-zA-Z_0-9]{0,22}[-a-zA-Z_0-9]))$";
            public const string StartWithHttp = "^(http|https)://((?!\\s\\S).)*$";
        }
        
        public class ValidationMessages
        {
            public const string Email = "Email field is not a valid e-mail address.";
            public const string StartWithHttp = "Please enter a valid URL (including http:// or https:// at the beginning).";
        }
    }
}
