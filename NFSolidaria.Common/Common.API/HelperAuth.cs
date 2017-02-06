using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Common.API
{
    public static class HelperAuth
    {

        public static string GetHeaderToken()
        {
            var token = HttpContext.Current.Request.Headers.Get("token");
            return string.IsNullOrEmpty(token) ? string.Empty : token;
        }


        public static string GetCookieToken()
        {

            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];

            if (cookieReq != null)
                return cookieReq.Values[0].ToString();


            return string.Empty;
        }

        public static string SetCookieToken(string token)
        {
            return SetCookieToken(token, "UserCookieAuthentication");
        }
        public static string SetCookieToken(string token, string cookieName)
        {
            ExpiresCookie(cookieName);
            var UserCookieAuthentication = new HttpCookie(cookieName);
            UserCookieAuthentication.Value = token;
            HttpContext.Current.Response.Cookies.Add(UserCookieAuthentication);
            UserCookieAuthentication.Expires = DateTime.Now.AddYears(1);
            var cookie = GetCookieToken();
            return cookie;

        }

        public static void ExpiresCookieToken()
        {
            ExpiresCookie("UserCookieAuthentication");
        }
        public static void ExpiresCookie(string cookieName)
        {

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                var UserCookieAuthentication = new HttpCookie(cookieName);
                UserCookieAuthentication.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(UserCookieAuthentication);
                HttpContext.Current.Response.Cookies.Remove(cookieName);

            }

        }

        public static void RemoveCookieToken()
        {
            RemoveCookie("UserCookieAuthentication");
        }
        public static void RemoveCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpContext.Current.Response.Cookies[cookieName].Value = null;
                HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddMonths(-1);
            }
        }

    }
}
