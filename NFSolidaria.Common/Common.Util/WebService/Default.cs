using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Util.WebService
{
    public class Default
    {
        public static String GetHeader()
        {
            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            sbHeader.Append("<{0} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            return sbHeader.ToString();
        }

        public static String GetFooter()
        {
            return "</{0}>";
        }

        public static String GetPattnerTag()
        {
            return "<{0}>{1}</{0}>";
        }

        public static String GetPattnerCDataTag()
        {
            return "<{0}><![CDATA[{1}]]></{0}>";
        }

        public static String GetBeginTag()
        {
            return "<{0}>";
        }

        public static String GetEndTag()
        {
            return "</{0}>";
        }

        public static String GetUserIPAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}
