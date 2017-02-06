using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Util.Tooltip
{
    public class ToolTipHelper
    {
        /// <summary>
        /// Adiciona eventos para o funcionamento do ToolTip no web Control Passado
        /// </summary>
        /// <param name="wbcItem">WebControl a ser adicionado o ToolTip</param>
        /// <param name="strImagem">Caminho da Imagem do ToolTip</param>
        /// <param name="intWidth">Largura da imagem</param>
        /// <param name="intHeight">Altura da imagem</param>
        /// <returns></returns>
        public static void GetToolTip(ref WebControl wbcItem, String strImagem, int intWidth, int intHeight)
        {
            wbcItem.Attributes.Clear();
            wbcItem.Attributes.Add("onClick", "return false;");
            wbcItem.Attributes.Add("onmousemove", "showlightbox(this,'" + strImagem + "'," + intWidth + "," + intHeight + ")");
        }

        /// <summary>
        /// Monta o tooltip para ser usado com o lightbox do JQuery
        /// </summary>
        /// <param name="strLinkHref">link da imagem</param>
        /// <param name="strLinkTitle">título da imagem</param>
        /// <param name="strImgSrc">caminho da imagem que aparecerá no ícone</param>
        /// <param name="strImgWidth">largura da imagem</param>
        /// <param name="strImgHeigth">altura da imagem</param>
        /// <param name="strImgAlt">texto que aparecerá no alt da imagem </param>
        /// <returns></returns>
        public static String GetJQueryTooltip(String strLinkHref, String strLinkTitle, String strImgSrc, String strImgWidth, String strImgHeigth, String strImgAlt)
        {
            String strPattenerLink = "<a href=\"{0}\" title=\"{1}\" rel=\"lightbox\">";
            String strPattenerImage = "<img src=\"{0}\" width=\"{1}\" height=\"{2}\" alt=\"{3}\" /></a>";

            StringBuilder sbTooltip = new StringBuilder();
            sbTooltip.Append(String.Format(strPattenerLink, strLinkHref, strLinkTitle));
            sbTooltip.Append(String.Format(strPattenerImage, strImgSrc, strImgWidth, strImgHeigth, strImgAlt));
            return sbTooltip.ToString();
        }
    }
}
