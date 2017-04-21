using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Web.Code.Infrastructure
{
    public class NotDefaultValueRequiredAttribute : ValidationAttribute
    {
        public NotDefaultValueRequiredAttribute()
            : base("The {0} property is required.")
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (value is string)
                return !string.IsNullOrWhiteSpace(value as string);

            var t = value.GetType();
            if (t.IsValueType)
            {
                var nullabelType = Nullable.GetUnderlyingType(t);
                Type type = nullabelType ?? t;

                var res = !value.Equals(Activator.CreateInstance(type));
                return res;
            }

            return true;
        }
    }
}
