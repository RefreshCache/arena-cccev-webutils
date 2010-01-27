using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Arena.Custom.Cccev.WebUtils.Security
{
    public static class CccevTextTools
    {
        /// <summary>
        /// Escapes all '<', '>', '&', '"', ''' in the input string and escapes them to their HTML equivalent.
        /// </summary>
        /// <param name="input">Unencoded string</param>
        /// <returns>Escaped HTML string</returns>
        public static string HtmlEncode(string input)
        {
            string output = string.Empty;
            output = HttpUtility.HtmlEncode(output);
            output = output.Replace("\"", "&quot;");
            output = output.Replace("'", "&#39;");
            return output;
        }

        /// <summary>
        /// Decodes "&lt;", "&gt;", "&amp;", "&quot;", "&#39;" in the input string and decodes it to it's plain text equivalent.
        /// </summary>
        /// <param name="input">HTML encoded string</param>
        /// <returns>Plain text string</returns>
        public static string HtmlDecode(string input)
        {
            string output = string.Empty;
            output = output.Replace("&quot;", "\"");
            output = output.Replace("&#39;", "'");
            return HttpUtility.HtmlEncode(output);
        }
    }
}
