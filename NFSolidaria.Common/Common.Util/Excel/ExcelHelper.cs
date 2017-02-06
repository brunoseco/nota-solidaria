using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace Util.Excel
{
    public class ExcelHelper
    {
        #region .: Constantes :.
        /// <summary>
        /// Parametro 1 - Nome da table;
        /// Parametro 2 - Valores a serem inseridos;
        /// </summary>
        private const string strPattenerInsert = "Insert into [{0}$] Values ({1});";
        /// <summary>
        /// Parametro 1 - Nome da table;
        /// Parametro 2 - Campos da tabela;
        /// </summary>
        private const string strPattnerCreateTable = "Create table {0} ({1});";
        #endregion

        #region .: Variables :.
        private String cadeiaConexao;
        private OleDbConnection conexao;
        private String strNomeArquivo;
        private String strCaminhoArquivo;
        private int intMaxQuantidadePorAba = 65535;
        #endregion

        #region .: Property´s :.
        /// <summary>
        /// Quantidade Máxima de registros por Aba
        /// </summary>
        public int MaxQuantidadePorAba
        {
            get
            {
                return intMaxQuantidadePorAba;
            }
            set
            {
                intMaxQuantidadePorAba = value;
            }
        }
        /// <summary>
        /// Retorna o nome do arquivo gerado
        /// </summary>
        public String NomeAquivo
        {
            get
            {
                return strNomeArquivo;
            }
        }
        /// <summary>
        /// Retorna o Caminho onde o arquivo será salvo Exemplo "C:\Excel\"
        /// </summary>
        public String CaminhoArquivo
        {
            get
            {
                return strCaminhoArquivo;
            }
        }
        #endregion

        #region .: Construtor :.
        public ExcelHelper(String strCaminhoArquivo)
        {
            this.strNomeArquivo = this.GenerateNomeArquivo(this.GenerateNomeArquivo());
            this.strCaminhoArquivo = strCaminhoArquivo;
        }
        public ExcelHelper(String strNomeArquivo, String strCaminhoArquivo)
        {
            this.strNomeArquivo = this.GenerateNomeArquivo(strNomeArquivo);
            this.strCaminhoArquivo = strCaminhoArquivo;
        }
        #endregion

        #region .: Methods :.
        protected String GenerateNomeArquivo(string strNomeArquivo)
        {
            return (strNomeArquivo + "_" + DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year.ToString("0000") + "_" + DateTime.Now.Hour.ToString("00") + "_" + DateTime.Now.Minute.ToString("00") + "_" + DateTime.Now.Second.ToString("00") + ".xls");
        }
        protected String GenerateNomeArquivo()
        {
            return (Guid.NewGuid().ToString());
        }
        public void Exportar(String NomePasta, String Campos, List<String> Dados)
        {
            if (this.CriaConexaoExcel())
            {
                this.ExportaDadosExcel(NomePasta, Campos, Dados);
                this.FechaConexaoExcel();
            }
        }
        public void AddAba(String strNomePasta, String strCampos, List<String> strDados)
        {
            if (this.CriaConexaoExcel())
            {
                this.GeraAbaExcel(strNomePasta, strCampos, strDados);
                this.FechaConexaoExcel();
            }
        }
        protected void ExportaDadosExcel(String strNomePasta, String strCampos, List<String> strDados)
        {
            if (strDados.Count > this.MaxQuantidadePorAba)
            {
                Double dblAbas = (Convert.ToDouble(strDados.Count) / this.MaxQuantidadePorAba);
                int intAbas = Convert.ToInt32(Math.Ceiling(dblAbas));
                List<String> strDadosAux;
                int intQtde;
                for (int i = 1; i <= intAbas; i++)
                {
                    strDadosAux = new List<String>();
                    if (strDados.Count < this.MaxQuantidadePorAba)
                        intQtde = strDados.Count;
                    else
                        intQtde = this.MaxQuantidadePorAba;
                    strDadosAux.AddRange(strDados.GetRange(0, intQtde));
                    strDados.RemoveRange(0, intQtde);
                    this.GeraAbaExcel(strNomePasta + "_" + i, strCampos, strDadosAux);
                }
            }
            else
                this.GeraAbaExcel(strNomePasta, strCampos, strDados);
        }
        protected void GeraAbaExcel(String strNomePlanilha, String strCampos, List<String> strDados)
        {
            if (conexao != null)
            {
                //Variávies
                StringBuilder sql = new StringBuilder();
                sql.Append(string.Format(strPattnerCreateTable, strNomePlanilha, strCampos));

                OleDbCommand query = new OleDbCommand(sql.ToString(), conexao);
                query.ExecuteNonQuery();

                strDados.ForEach(delegate(String _strDados)
                {
                    sql = new StringBuilder();

                    sql.Append(string.Format(strPattenerInsert, strNomePlanilha, _strDados));
                    query = new OleDbCommand(sql.ToString(), conexao);
                    query.ExecuteNonQuery();
                });
            }
        }
        #endregion

        #region .: Manipula Conexão :.
        protected Boolean CriaConexaoExcel()
        {
            try
            {
                String _CaminhoArquivo = this.CaminhoArquivo + this.NomeAquivo;
                cadeiaConexao = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _CaminhoArquivo + ";Extended Properties=Excel 8.0;";
                conexao = new OleDbConnection(cadeiaConexao);
                conexao.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void FechaConexaoExcel()
        {
            conexao.Close();// fechando a conexão
        }
        #endregion
    }
}
