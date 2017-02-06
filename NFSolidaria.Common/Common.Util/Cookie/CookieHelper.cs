using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Util.Cookie
{
    public class CookieHelper
    {
        /// <summary>
        /// Grava um cookie no computador do usuário
        /// </summary>
        /// <param name="strCookieName">nome do cookie</param>
        /// <param name="strFieldName">campo onde a informação será armazenada</param>
        /// <param name="strValue">informação que será armazenada</param>
        /// <param name="dteExpiresDate">data de expiração do cookie</param>
        public static void SetCookie(String strCookieName, String strFieldName, String strValue, DateTime dteExpiresDate)
        {
            SetCookie(strCookieName, strFieldName, strValue, dteExpiresDate,
                HttpContext.Current.Request.IsSecureConnection);
        }

        /// <summary>
        /// Grava um cookie no computador do usuário
        /// </summary>
        /// <param name="strCookieName">nome do cookie</param>
        /// <param name="strFieldName">campo onde a informação será armazenada</param>
        /// <param name="strValue">informação que será armazenada</param>
        /// <param name="dteExpiresDate">data de expiração do cookie</param>
        /// <param name="blnSecure">cookie seguro</param>
        public static void SetCookie(String strCookieName, String strFieldName, String strValue, DateTime dteExpiresDate, bool blnSecure)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[strCookieName];

            if (myCookie == null)
                myCookie = new HttpCookie(strCookieName);

            myCookie[strFieldName] = strValue;
            myCookie.Expires = dteExpiresDate;
            myCookie.Secure = blnSecure;
            myCookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        /// <summary>
        /// Recupera o valor de um determinado campo de um determinado cookie
        /// </summary>
        /// <param name="strCookieName">nome do cookie de onde a informação será recuperada</param>
        /// <param name="strFieldName">campo do cookie de onde a informação será recuperada</param>
        /// <returns></returns>
        public static String GetCookie(String strCookieName, String strFieldName)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[strCookieName];

            if (myCookie != null)
                return myCookie[strFieldName];

            return null;
        }

        /// <summary>
        /// Faz com que um determinado cookie expire
        /// </summary>
        /// <param name="strCookieName">nome do cookie que será expirado</param>
        /// <returns>true se o cookie foi expirado com sucesso e false caso contrário</returns>
        public static Boolean ExpiresCookie(String strCookieName)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[strCookieName];

            if (myCookie != null)
            {
                myCookie.Value = "";
                HttpContext.Current.Response.Cookies.Set(myCookie);
                return true;
            }
            return false;
        }
    }
}
