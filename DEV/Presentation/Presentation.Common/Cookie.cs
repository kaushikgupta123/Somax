using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SOMAX.G4.Business.Common;
using System.IO;

namespace SOMAX.G4.Presentation.Common
{
    public class Cookie
    {

        /// <summary>Get the value from a cookie and perform decryption on it</summary>
        /// <param name="name">The name of the cookie</param>
        /// <param name="encryptionKey">The key used for decryption</param>
        /// <returns>The decrypted string</return>
        public static string GetDecrypted(string name, string key, string encryptionKey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];

            string decryptedData = string.Empty;

            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(cookie[key]))
                {
                    decryptedData = Encryption.DecriptString(cookie[key], encryptionKey);
                }
                else if (cookie.Value.StartsWith(key))
                {
                    // Note that jQuery will URI Encode strings to the cookie, which will mess with 
                    // .NET's ability to detect key/value pairs in the cookie. Functions like the Quick Find
                    // will fail because of this, since the equal sign (=) gets encoded. If we detect that 
                    // the value starts with the string we're expecting, we'll try to manually decode it here
                    // instead of relying upon .NET to find the key for us.
                    string decoded = Uri.UnescapeDataString(cookie.Value);
                    decryptedData = Encryption.DecriptString(decoded.Substring(key.Length + 1), encryptionKey);
                }
            }

            return decryptedData;
        }

        /// <summary>Perform encryption on a string value and set it in a cookie</summary>
        /// <param name="name">The name of the cookie</param>
        /// <param name="data">The string to be encrypted</param>
        /// <param name="encryptionKey">The key used for encryption</param>
        public static void SetEncrypted(string name, string key, string data, string encryptionKey)
        {
            string encryptedStr = Encryption.EncryptString(data, encryptionKey);
            
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            //if (cookie != null) { Remove(name); }

            if (cookie == null) { cookie = new HttpCookie(name); }
            cookie[key] = encryptedStr;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>Get value from a cookie</summary>
        /// <param name="name">The name of the cookie</param>
        /// <param name="data">The string to be retrieved</param>
        public static string Get(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];

            if (cookie != null)
            { 
                return HttpContext.Current.Server.UrlDecode(cookie.Value); 
            }
            else
            { 
                return string.Empty; 
            }
        }

        /// <summary>Set value into a cookie</summary>
        /// <param name="name">The name of the cookie</param>
        /// <param name="data">The string to be set</param>
        public static void Set(string name, string data)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null) { Remove(name); }

            cookie = new HttpCookie(name);
            cookie.Value = HttpContext.Current.Server.UrlEncode(data);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

      /// <summary>
      ///  Set Value into a cookie
      /// </summary>
      /// <param name="name">The name of the cookie</param>
      /// <param name="data">The string to be set</param>
      /// <param name="Days">No of days after which cookie expires</param>
        public static void Set(string name, string data,int Days)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null) { Remove(name); }

            cookie = new HttpCookie(name);
            cookie.Value = HttpContext.Current.Server.UrlEncode(data);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>Delete a cookie</summary>
        /// <param name="pageName">The name of the cookie</param>
        public static void Remove(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>Return the name of the cookie for a specific page</summary>
        /// <param name="pageName">The name of the page</param>
        public static string  GeneratePageCookieName(string pageName)
        {
            // pageName should be like Equipment.aspx
            return pageName.Replace(".", "_");
        }

        /// <summary>Delete a page cookie</summary>
        /// <param name="pageName">The name of the page</param>
        public static void RemovePageCookie(string pageName)
        {
            Cookie.Remove(Cookie.GeneratePageCookieName(Path.GetFileName(pageName)));
        }
    }
}
