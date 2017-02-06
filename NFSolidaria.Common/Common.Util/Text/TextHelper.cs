using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Util.Text
{
    public class TextHelper
    {
        /// <summary>
        /// Remove todos os assentos dos caracteres da String passada
        /// </summary>
        /// <param name="strTexto">Texto</param>
        /// <returns>O texto passado sem acentos nos caracteres</returns>
        public static string RemoveAcentos(String strTexto)
        {
            strTexto = strTexto.Replace(" ", "-");
            strTexto = strTexto.Replace(",", "-");
            strTexto = strTexto.Replace(".", "-");
            strTexto = strTexto.Replace(":", "-");
            strTexto = strTexto.Replace(";", "-");

            strTexto = strTexto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in strTexto.ToCharArray())
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            strTexto = sb.ToString();
            strTexto = Regex.Replace(strTexto, "[^0-9a-zA-Z_-]+?", "");
            strTexto = strTexto.Replace("---", "-");
            strTexto = strTexto.Replace("--", "-");

            return strTexto;
        }
        /// <summary>
        /// Remove todos os caracteres especiais da String passada
        /// </summary>
        /// <param name="strTexto">Texto</param>
        /// <returns>O texto passado sem caracteres especiais</returns>
        private static string RemoveCaracteresEspeciais(String strTexto)
        {
            String strResult = "";

            // Remove os outros caracteres especiais.
            strResult = Regex.Replace(strTexto, "[^0-9a-zA-Z_]+?", "_");

            return strResult;
        }

        public static string MontaFimUrlRW(String strNome)
        {
            return (HttpUtility.UrlDecode(RemoveAcentos(strNome)) + ".aspx").ToLower();
        }

        /// <summary>
        /// Remove TAGS HTML da String passada
        /// </summary>
        /// <param name="strTexto">Texto</param>
        /// <returns>O texto passado sem TAGS HTML</returns>
        public static String RemoveTagsHtml(string texto)
        {
            string result = null;

            //Remove tags html
            result = Regex.Replace(texto, "<[^>]*>", string.Empty, RegexOptions.IgnoreCase);

            //Remove espaços duplicados
            result = Regex.Replace(result, @"\s+", " ", RegexOptions.IgnoreCase);

            return result;
        }

    }
}
