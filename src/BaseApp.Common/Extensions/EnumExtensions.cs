using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BaseApp.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns description of enum item marked with "Description" attribute
        /// </summary>
        public static string ToDescription(this Enum en) //ext method
        {
            var descriptionAttribute = GetAttributeOrDefault<DescriptionAttribute>(en);
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            /* to enable this you should add dependency Microsoft.AspNet.Mvc.DataAnnotations
            var displayAttribute = GetAttributeOrDefault<DisplayAttribute>(en);
            if (displayAttribute != null)
            {
                return displayAttribute.Name;
            }
            */

            var displayNameAttribute = GetAttributeOrDefault<DisplayNameAttribute>(en);
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }
            
            return en.ToString();
        }

        public static List<KeyValuePair<T, String>> EnumToList<T>()
        {
            Type enumType = typeof(T);
            Array enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<KeyValuePair<T, String>>(enumValArray.Length);
            foreach (int val in enumValArray)
            {
                var item = Enum.Parse(enumType, val.ToString());
                enumValList.Add(new KeyValuePair<T, String>((T)item, ((Enum)item).ToDescription()));
            }
            return enumValList;
        }

        public static T GetAttribute<T>(this Enum enumeration)
            where T : Attribute
        {
            var attribute = GetAttributeInner<T>(enumeration);
            if (attribute == null)
                throw new Exception($"Attribute {typeof (T)} not specified for enum {enumeration}");

            return attribute;
        }

        public static T GetAttributeOrDefault<T>(this Enum enumeration)
            where T : Attribute
        {
            return GetAttributeInner<T>(enumeration);
        }

        private static T GetAttributeInner<T>(Enum enumeration) where T : Attribute
        {
            var members = enumeration
                .GetType()
                .GetMember(enumeration.ToString()).ToList();
            if (members.Count <= 0)
                throw new Exception($"Value {enumeration} does not belong to enumeration {enumeration.GetType()}");

            return members[0]
                .GetCustomAttributes(typeof(T), false)
                .Cast<T>()
                .SingleOrDefault();
        }

        public static TExpected GetAttributeValue<T, TExpected>(this Enum enumeration, Func<T, TExpected> expression)
            where T : Attribute
        {
            return GetAttributeValueInner(enumeration, expression, false);
        }

        public static TExpected GetAttributeValueOrDefault<T, TExpected>(this Enum enumeration, Func<T, TExpected> expression)
            where T : Attribute
        {
            return GetAttributeValueInner(enumeration, expression, true);
        }

        private static TExpected GetAttributeValueInner<T, TExpected>(Enum enumeration, Func<T, TExpected> expression, bool defaultAllowed) where T : Attribute
        {
            T attribute = defaultAllowed
                ? enumeration.GetAttributeOrDefault<T>()
                : enumeration.GetAttribute<T>();

            if (attribute == null)
                return default(TExpected);

            return expression(attribute);
        }
    }
}