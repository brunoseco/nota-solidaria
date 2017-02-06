using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Util.Log
{
    public class LogHelper
    {
        /// <summary>
        /// Gera arquivo de Log do Sistema
        /// </summary>
        /// <param name="strPathFisico">A pasta que será salvo e o nome do arquivo</param>
        /// <param name="strMensagem">Mensagem de erro</param>
        public static void GeraArquivoLog(String strPathFisico, String strMensagem)
        {
            StreamWriter swAscx = new StreamWriter(strPathFisico, true);
            swAscx.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " | " + strMensagem);
            swAscx.Close();
            swAscx.Dispose();
        }

        /// <summary>
        /// Gera arquivo de Log do Sistema Por Exception
        /// </summary>
        /// <param name="strPathFisico">A pasta que será salvo e o nome do arquivo</param>
        /// <param name="ex">Exception</param>
        /// <param name="strTitulo">Titulo do Erro</param>
        public static void GeraLogException(String strPathFisico, Exception ex, String strTitulo)
        {
            StreamWriter swAscx = new StreamWriter(strPathFisico + strTitulo + ".txt", true);
                
            swAscx.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " | Erro Pagamento Digital");
            swAscx.WriteLine("Mensagem: " + ex.Message);
            swAscx.WriteLine("StackTrace: " + ex.StackTrace);
            swAscx.WriteLine("========================================");

            swAscx.Close();
            swAscx.Dispose();
        }

        /// <summary>
        /// Gera arquivo de Log do Sistema Por Exception
        /// </summary>
        /// <param name="strPathFisico">A pasta que será salvo e o nome do arquivo</param>
        /// <param name="ex">Exception</param>
        /// <param name="strTitulo">Titulo do Erro</param>
        public static void GeraLogException(String strPathFisico, Exception ex)
        {
            StreamWriter swAscx = new StreamWriter(strPathFisico, true);
            swAscx.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " | FALHA NA EXECUÇÃO DO JobIntegracaoProdutos");
            swAscx.WriteLine("Mensagem: " + ex.Message);
            swAscx.WriteLine("StackTrace: " + ex.StackTrace);
            swAscx.WriteLine("========================================");

            swAscx.Close();
            swAscx.Dispose();
        }
    }
}
