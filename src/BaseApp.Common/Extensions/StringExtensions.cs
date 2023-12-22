using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BaseApp.Common.Extensions
{
    public static class StringExtensions
    {
        public static string LimitedTo(this string text, int length)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text.Length > length ? text.Substring(0, length).Trim() + "..." : text;
        }

        public static string HtmlStrip(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            string htmlStrip = Regex.Replace(html, "<[^>]*>", string.Empty);
            htmlStrip = htmlStrip.Replace("&nbsp;", " ");

            const char newLine = (char)10;
            htmlStrip = htmlStrip.TrimEnd(new[] { newLine });
            return htmlStrip;
        }

        public static bool EqualsIgnoreCase(this string str, string compared)
        {
            return string.Equals(str, compared, StringComparison.OrdinalIgnoreCase);
        }
        
        public static string UseForJoinNonEmptyObjects<T>(this string separator, IEnumerable<T> items)
        {
            if (items == null)
                return "";
            return separator.UseForJoinNonEmpty(items.Select(x => x.ToString()).ToArray());
        }

        public static string UseForJoinNonEmpty(this string separator, IEnumerable<string> items)
        {
            return items == null 
                ? "" 
                : separator.UseForJoinNonEmpty(items.ToArray());
        }

        public static string UseForJoinNonEmpty(this string separator, params string[] items)
        {
            if (items.IsNullOrEmpty())
                return "";
            return string.Join(separator, items.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
