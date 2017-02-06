using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Globalization;

namespace Util.Data
{
    public class DataHelper
    {
        /// <summary>
        /// Verifica se a data é igual à DateTime.MinValue ou igual a SqlDateTime.MinValue.
        /// </summary>
        /// <param name="clsData">Data que será verificada</param>
        /// <returns>true se a data está vazia e false caso ela exista</returns>
        public static Boolean IsEmpty(DateTime clsData)
        {
            return (clsData.Equals(DateTime.MinValue) || clsData.ToShortDateString().Equals(DateTime.MaxValue.ToShortDateString()) ||
                    clsData.ToString().Equals(SqlDateTime.MinValue.ToString())
                    );
        }

        /// <summary>
        /// Verifica se as datas são iguais.
        /// </summary>
        /// <returns>true se as datas são iguais e false caso contrário</returns>
        public static Boolean Equals(DateTime clsData1, DateTime clsData2)
        {
            return clsData1.ToShortDateString().Equals(clsData2.ToShortDateString());
        }

        /// <summary>
        /// Retorna o primeiro dia da semana
        /// </summary>
        /// <param name="clsData"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime clsData)
        {
            switch (clsData.DayOfWeek)
            {
                case DayOfWeek.Sunday: return clsData;
                case DayOfWeek.Monday: return clsData.AddDays(-1);
                case DayOfWeek.Tuesday: return clsData.AddDays(-2);
                case DayOfWeek.Wednesday: return clsData.AddDays(-3);
                case DayOfWeek.Thursday: return clsData.AddDays(-4);
                case DayOfWeek.Friday: return clsData.AddDays(-5);
                case DayOfWeek.Saturday: return clsData.AddDays(-6);
                default: return clsData;
            }
        }

        /// <summary>
        /// Retorna a data recebida por extenso. Por exemplo:
        /// recebe: 26/06/2009 e retorna "26 de junho de 2009".
        /// </summary>
        public static String GetFullDate(DateTime clsData)
        {
            return clsData.ToString("dd") + " de " + clsData.ToString("MMMMM") + " de " + clsData.ToString("yyyy");
        }

        /// <summary>
        /// Valida se a formatação MM/yyyy está correta
        /// </summary>
        /// <returns></returns>
        public static Boolean ValidationMonthAndYear(String strMonthYear)
        {
            return Regex.IsMatch(strMonthYear, @"^((0[1-9]|1[0-2])\/(19\d{2}|2\d{3})|(__\/____))$");
        }

        /// <summary>
        /// Tenta converter a string passada em DateTime
        /// </summary>
        /// <param name="strData">string que será convertida para DateTime</param>
        /// <returns>a string convertida em DateTime ou DateTime.MinValue caso não seja possível converter</returns>
        public static DateTime TryParse(String strData)
        {
            DateTime dteData = DateTime.MinValue;
            DateTime.TryParse(strData, out dteData);
            return dteData;
        }
        /// <summary>
        /// Tenta converter a string passada em DateTime
        /// </summary>
        /// <param name="strData">string que será convertida para DateTime</param>
        /// <returns>a string convertida em DateTime ou DateTime.MinValue caso não seja possível converter</returns>
        public static DateTime TryParseDatabase(String strData)
        {
            DateTime dteData = DateTime.MinValue;
            DateTime.TryParse(strData, out dteData);

            if (dteData.Equals(DateTime.MinValue))
                return GetDatabaseMinValue();
            return dteData;
        }
        /// <summary>
        /// Retorna o mínimo valor de data que o banco de dados aceita convertido em DateTime
        /// </summary>
        public static DateTime GetDatabaseMinValue()
        {
            return (DateTime)SqlDateTime.MinValue;
        }

        public static DateTime GetByDataInt(String strData)
        {
            try
            {
                String strDia = strData.Remove(2);
                String strMes = strData.Remove(4).Remove(0, 2);
                String strAno = strData.Remove(0, 4);
                String strDateTime = strDia + "/" + strMes + "/" + strAno;
                return Convert.ToDateTime(strDateTime);
            }
            catch
            {
                return GetDatabaseMinValue();
            }
        }

        public static DateTime GetByDataIntHoraInt(String strData, String strHora)
        {
            try
            {
                String strDia = strData.Remove(2);
                String strMes = strData.Remove(4).Remove(0, 2);
                String strAno = strData.Remove(0, 4);

                String strHora1 = strHora.Remove(2);
                String strMinuto = strHora.Remove(4).Remove(0, 2);
                String strSegundo = strHora.Remove(0, 4);

                String strDateTime = strDia + "/" + strMes + "/" + strAno + " " + strHora1 + ":" + strMinuto + ":" + strSegundo;
                return Convert.ToDateTime(strDateTime);
            }
            catch
            {
                return GetDatabaseMinValue();
            }
        }

        /// <summary>
        /// Tenta converter a string passada em DateTime
        /// </summary>
        /// <param name="strData">string que será convertida para DateTime</param>
        /// <returns>a string convertida em DateTime ou DateTime.MinValue caso não seja possível converter</returns>
        public static DateTime TryParsePadraoBrasil(String strData)
        {
            DateTime dteData = DateTime.MinValue;
            DateTime.TryParse(strData, new CultureInfo("pt-BR"), DateTimeStyles.None, out dteData);
            return dteData;
        }

    }
}
