using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Util.Request
{
    public class RequestHelper
    {
        /// <summary>
        /// Busca Query String cm Tratamento
        /// </summary>
        /// <param name="strKey">Key da Query String a ser tratada</param>
        /// <returns>Valor da strKey se existir</returns>
        public static String ReturnQueryString(String strKey)
        {
            String strReturn = "";
            HttpContext context = HttpContext.Current;
            if (context.Request.QueryString[strKey] != null)
                strReturn = context.Request.QueryString[strKey].ToString();
            return strReturn;
        }
        /// <summary>
        /// Busca o IP do usuário
        /// </summary>
        /// <returns>IP</returns>
        public static String GetIP()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        /// <summary>
        /// Busca o AbsoluteUri
        /// </summary>
        /// <returns>Valor da AbsoluteUri</returns>
        public static String ReturnAbsoluteUri()
        {
            HttpContext context = HttpContext.Current;
            return context.Request.Url.AbsoluteUri;
        }
        /// <summary>
        /// Pega a URL
        /// </summary>
        /// <returns></returns>
        public static String ReturnURL()
        {
            HttpContext context = HttpContext.Current;
            return context.Request.Url.ToString();
        }
    }
}
