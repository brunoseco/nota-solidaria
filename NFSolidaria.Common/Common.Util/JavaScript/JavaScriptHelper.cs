using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Util.JavaScript
{
    public class JavaScriptHelper
    {
        #region .: CommandsJS :.
        /// <summary>
        /// Monta Mensagem ao Cliente
        /// </summary>
        /// <param name="strMessage">Mensagem a ser exibida</param>
        /// <param name="blnAjax">Confirma se existe Update Panel na página</param>
        /// <returns></returns>
        public static void Message(String strMessage, Boolean blnAjax)
        {
            HttpContext context = HttpContext.Current;
            Page page = (Page)context.Handler;

            if (blnAjax)
                ScriptManager.RegisterStartupScript(page, typeof(Page), Guid.NewGuid().ToString(), Message(strMessage), false);
            else
                page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), Message(strMessage));
        }

        /// <summary>
        /// Monta Alert ao Cliente
        /// </summary>
        /// <param name="strMessage">Mensagem a ser exibida</param>
        /// <param name="blnAjax">Confirma se existe Update Panel na página</param>
        /// <returns></returns>
        public static void Alert(String strMessage, Boolean blnAjax)
        {
            HttpContext context = HttpContext.Current;
            Page page = (Page)context.Handler;

            if (blnAjax)
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Alert", MessageAlerta(strMessage), false);
            else
                page.ClientScript.RegisterStartupScript(page.GetType(), "Alert", MessageAlerta(strMessage));
        }

        /// <summary>
        /// Monta Confirm ao Cliente
        /// </summary>
        /// <param name="strMessage">Mensagem a ser exibida</param>
        /// <param name="blnAjax">Confirma se existe Update Panel na página</param>
        /// <returns></returns>
        public static void Confirm(String strMessage, Boolean blnAjax)
        {
            HttpContext context = HttpContext.Current;
            Page page = (Page)context.Handler;

            if (blnAjax)
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Confirm", MessageAlerta(strMessage), false);
            else
                page.ClientScript.RegisterStartupScript(page.GetType(), "Confirm", MessageComfirm(strMessage));
        }

        /// <summary>
        /// Monta Confirm ao Cliente
        /// </summary>
        /// <param name="strMessage">Mensagem a ser exibida</param>
        /// <param name="blnAjax">Confirma se existe Update Panel na página</param>
        /// <param name="strConfirmName">Nome da janela de confirm usada para pegar o seu resultado posteriormente</param>
        /// <returns></returns>
        public static void ConfirmWithResult(String strMessage, Boolean blnAjax, String strConfirmName)
        {
            HttpContext context = HttpContext.Current;
            Page page = (Page)context.Handler;

            if (blnAjax)
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Confirm", MessageConfirmWithResult(strMessage, strConfirmName), false);
            else
                page.ClientScript.RegisterStartupScript(page.GetType(), "Confirm", MessageConfirmWithResult(strMessage, strConfirmName));
        }

        public static Boolean GetConfirmResult(String strConfirmName)
        {
            HttpContext context = HttpContext.Current;
            Page page = (Page)context.Handler;

            string eventTarget = (page.Request["__EVENTTARGET"] == null) ? string.Empty : page.Request["__EVENTTARGET"];
            string eventArgument = (page.Request["__EVENTARGUMENT"] == null) ? string.Empty : page.Request["__EVENTARGUMENT"];

            if (eventTarget == strConfirmName)
            {
                if (eventArgument == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region .: Auxiliar Methods :.
        private static String Message(String strMensagem)
        {
            string strMessage = "<script language=\"javascript\" type=\"text/javascript\" >{0}</script>";
            return strMessage = string.Format(strMessage, strMensagem);
        }

        private static String MessageAlerta(String strMensagem)
        {
            string strAlerta = "<script language=\"javascript\" type=\"text/javascript\" >alert('{0}')</script>";
            return strAlerta = string.Format(strAlerta, strMensagem);
        }

        private static String MessageComfirm(String strMensagem)
        {
            string strConfirm = "<script language=\"javascript\" type=\"text/javascript\" >return if(confirm('{0}'))</script>";
            return strConfirm = string.Format(strConfirm, strMensagem);
        }
        private static String MessageConfirmWithResult(String strMensagem, String strConfirmName)
        {
            StringBuilder myScript = new StringBuilder("");
            myScript.Append("<script type='text/javascript' language='javascript'>");
            myScript.Append("var result = window.confirm('" + strMensagem + "');");
            myScript.Append("__doPostBack('" + strConfirmName + "', result);");
            myScript.Append("</script>");

            return myScript.ToString();
        }
        #endregion
    }
}