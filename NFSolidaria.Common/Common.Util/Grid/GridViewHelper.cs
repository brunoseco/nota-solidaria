using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Util.Grid
{
    public class GridViewHelper
    {
        /// <summary>
        /// Retorna o id referente � linha selecionada.
        /// </summary>
        /// <param name="gvwGrid">grid view</param>
        /// <param name="strControlName">nome do controle que ser� verificado</param>
        /// <returns>o id da linha selecionada</returns>
        public static int GetIDSelecionado(GridView gvwGrid, String strControlName)
        {
            foreach (GridViewRow linha in gvwGrid.Rows)
            {
                if (linha.RowType == DataControlRowType.DataRow)
                {
                    CheckBox rblSelecionado = (CheckBox)linha.FindControl(strControlName);
                    if (rblSelecionado.Checked)
                        return (int)gvwGrid.DataKeys[linha.RowIndex].Value;
                }
            }
            return 0;
        }

        public static Int64 GetIDSelecionadoInt64(GridView gvwGrid, String strControlName)
        {
            foreach (GridViewRow linha in gvwGrid.Rows)
            {
                if (linha.RowType == DataControlRowType.DataRow)
                {
                    CheckBox rblSelecionado = (CheckBox)linha.FindControl(strControlName);
                    if (rblSelecionado.Checked)
                        return (Int64)gvwGrid.DataKeys[linha.RowIndex].Value;
                }
            }
            return 0;
        }

        /// <summary>
        /// Retira a sele��o de todos os controles com o nome recebido por par�metro dentro da grid view.
        /// </summary>
        /// <param name="gvwGrid">grid view</param>
        /// <param name="strControlName">nome do controle que ser� deselecionado</param>
        public static void DeselectAll(GridView gvwGrid, String strControlName)
        {
            DeselectAllExceptOne(gvwGrid, null, strControlName);
        }

        /// <summary>
        /// Retira a sele��o de todos os outros controles existentes no Grid, deixando apenas um selecionado
        /// </summary>
        /// <param name="gvwGrid">grid view</param>
        /// <param name="SelectedLine">linha onde est� o controle que ser� mantido selecionado</param>
        /// <param name="strControlName">nome do controle que est� dentro da gid</param>
        public static void DeselectAllExceptOne(GridView gvwGrid, GridViewRow SelectedLine, String strControlName)
        {
            foreach (GridViewRow linha in gvwGrid.Rows)
            {
                CheckBox chk = (CheckBox)linha.FindControl(strControlName);

                if (SelectedLine == null)
                    chk.Checked = false;
                else if (linha.RowIndex != SelectedLine.RowIndex)
                    chk.Checked = false;
            }
        }

        /// <summary>
        /// Verifica se existe ao menos um controle selecionado no grid view
        /// </summary>
        /// <param name="gvwGrid">grid view</param>
        /// <param name="strControlName">nome do controle que ser� verificado</param>
        /// <returns>true se existe o menos um controle selecionado e false caso contr�rio</returns>
        public static Boolean ExistsSelected(GridView gvwGrid, String strControlName)
        {
            foreach (GridViewRow linha in gvwGrid.Rows)
            {
                CheckBox chk = (CheckBox)linha.FindControl(strControlName);
                if (chk.Checked) return true;
            }
            return false;
        }
    }
}
