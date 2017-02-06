using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Util.Flash
{
    public static class FlashHelper
    {
        public static String GetScript(String PathFlash, String Width, String Height, String FlashVars, Boolean Transparent, Boolean NoScale, Boolean bln100Percent)
        {
            StringBuilder flash = new StringBuilder();

            flash.Append("<script type='text/javascript'>" + Environment.NewLine);
            flash.Append("AC_FL_RunContent(" + Environment.NewLine);
            flash.Append("'codebase','https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0'," + Environment.NewLine);
            if (bln100Percent)
            {
                flash.Append("'width','" + "100%" + "'," + Environment.NewLine);
                flash.Append("'height','" + "100%" + "'," + Environment.NewLine);
            }
            else
            {
                flash.Append("'width','" + Width + "'," + Environment.NewLine);
                flash.Append("'height','" + Height + "'," + Environment.NewLine);
            }
            flash.Append("'src','" + PathFlash + "'," + Environment.NewLine);
            flash.Append("'quality','high'," + Environment.NewLine);
            flash.Append("'pluginspage','https://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash'," + Environment.NewLine);
            flash.Append("'movie','" + PathFlash + "'," + Environment.NewLine);

            if (Transparent == true)
                flash.Append("'wmode','transparent' ,");

            if (NoScale == true)
                flash.Append("'SCALE','noscale',");

            flash.Append("'FlashVars','" + FlashVars + "'" + Environment.NewLine);
            flash.Append(");" + Environment.NewLine);
            flash.Append("</script>" + Environment.NewLine);

            return flash.ToString();
        }

        public static String GetScript(String PathFlash, String Width, String Height, String FlashVars, Boolean Transparent, Boolean NoScale)
        {
            StringBuilder flash = new StringBuilder();

            flash.Append("<script type='text/javascript'>" + Environment.NewLine);
            flash.Append("AC_FL_RunContent(" + Environment.NewLine);
            flash.Append("'codebase','https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0'," + Environment.NewLine);
            flash.Append("'width','" + Width + "'," + Environment.NewLine);
            flash.Append("'height','" + Height + "'," + Environment.NewLine);
            flash.Append("'src','" + PathFlash + "'," + Environment.NewLine);
            flash.Append("'quality','high'," + Environment.NewLine);
            flash.Append("'pluginspage','https://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash'," + Environment.NewLine);
            flash.Append("'movie','" + PathFlash + "'," + Environment.NewLine);

            if (Transparent == true)
                flash.Append("'wmode','transparent' ,");

            if (NoScale == true)
                flash.Append("'SCALE','noscale',");

            flash.Append("'FlashVars','" + FlashVars + "'" + Environment.NewLine);
            flash.Append(");" + Environment.NewLine);
            flash.Append("</script>" + Environment.NewLine);

            return flash.ToString();
        }

        public static Boolean CreateFile(String PathXML, String strXML, String strNameXML)
        {
            if (!Directory.Exists(PathXML))
                Directory.CreateDirectory(PathXML);

            StreamWriter swFile;
            swFile = new StreamWriter(PathXML + strNameXML, false, Encoding.UTF8);
            swFile.WriteLine(strXML);
            swFile.Close();
            swFile.Dispose();
            return true;
        }
    }
}