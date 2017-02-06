using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Numero
{
    public class NumeroHelper
    {
        public static bool BoolTryParse(String strNumero)
        {
            bool blnNumero = false;
            bool.TryParse(strNumero, out blnNumero);
            return blnNumero;
        }
        /// <summary>
        /// Tenta converter a String passada em inteiro e retorna 0 se não conseguir
        /// </summary>
        /// <param name="strNumero">String a ser convertida em inteiro</param>
        /// <returns>O inteiro representado pela string ou 0 se a string passada não representar um inteiro</returns>
        public static int intTryParse(String strNumero)
        {
            int intNumero = 0;
            int.TryParse(strNumero, out intNumero);
            return intNumero;
        }
        /// <summary>
        /// Tenta converter a String passada em double e retorna 0 se não conseguir
        /// </summary>
        /// <param name="strNumero">String a ser convertida em double</param>
        /// <returns>O double representado pela string ou 0 se a string passada não representar um double</returns>
        public static Double DoubleTryParse(String strNumero)
        {
            Double dblNumero = 0;
            Double.TryParse(strNumero, out dblNumero);
            return dblNumero;
        }

        /// <summary>
        /// Tenta converter a String passada em decimal e retorna 0 se não conseguir
        /// </summary>
        /// <param name="strNumero">String a ser convertida em double</param>
        /// <returns>O decimal representado pela string ou 0 se a string passada não representar um decimal</returns>
        public static Decimal DecimalTryParse(String strNumero)
        {
            Decimal dblNumero = 0;
            Decimal.TryParse(strNumero, out dblNumero);
            return dblNumero;
        }

        /// <summary>
        /// Gera um número aleatório entre os parâmetros passados
        /// </summary>
        /// <param name="min">valor mínimo</param>
        /// <param name="max">valor máximo</param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            return new Random().Next(min, max);
        }

        /// <summary>
        /// Remove os números a partir da terceira casa após a virgula de um número 
        /// decimal sem arredondar o seu valor
        /// </summary>
        /// <param name="dblNumber">O numero</param>
        /// <returns>O numero formatado</returns>
        public static double RemoveFractionalPart(double dblNumber)
        {
            return RemoveFractionalPart(dblNumber, 2);
        }

        public static double RemoveFractionalPart(double dblNumber, int intCasasDecimais)
        {
            double dblFator = Math.Pow(10, intCasasDecimais);
            return Math.Truncate(dblNumber * dblFator) / dblFator;
        }
    }
}
