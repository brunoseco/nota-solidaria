using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Util.Session
{
    public class SessionHelper
    {
        /// <summary>
        /// Determina um valor para uma session
        /// </summary>
        /// <param name="strSessionName">nome da session</param>
        /// <param name="strValue">valor que será colocado na session</param>
        public static void SetValueToSession(String strSessionName, String strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
        }

        /// <summary>
        /// Retorna o valor de uma session
        /// </summary>
        /// <param name="strSessionName">nome da session</param>
        /// <returns>o valor da session</returns>
        public static String GetValueFromSession(String strSessionName)
        {
           // string sess = HttpContext.Current.Session["carrinho"].ToString();
            if (ExistsSession(strSessionName))
              return HttpContext.Current.Session[strSessionName].ToString();

            return strSessionName;
        }

        /// <summary>
        /// Verifica se uma session existe
        /// </summary>
        /// <param name="strSessionName">nome da session</param>
        /// <returns>true se a session existe e false caso contrário</returns>
        public static Boolean ExistsSession(String strSessionName)
        {
            return (HttpContext.Current.Session[strSessionName] != null);
        }

        /// <summary>
        /// Mata uma determinada session
        /// </summary>
        /// <param name="strSessionName">session que será morta</param>
        public static void KillSession(String strSessionName)
        {
            if (ExistsSession(strSessionName))
                HttpContext.Current.Session[strSessionName] = null;
        }
    }
}
