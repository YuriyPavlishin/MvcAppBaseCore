using System;
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
    }
}
