using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Format
{
    public class FormatHelper
    {
        public static String Money(Double dblMoney)
        {
            return dblMoney.ToString("#,###,###,##0.00");
        }

        public static String Money(Decimal dblMoney)
        {
            return dblMoney.ToString("#,###,###,##0.00");
        }

        public static String Date(DateTime dteDate)
        {
            return dteDate.ToString("dd/MM/yyyy");
        }

        public static String DateHour(DateTime dteDate)
        {
            return dteDate.ToString("dd/MM/yyyy HH:mm:ss");


        }

        public static String Phone(String Value)
        {
            if (String.IsNullOrEmpty(Value)) return Value;

            String Formato = "";
            if (Value.Length == 15)
                Formato = "(##) #####-####";
            else
                Formato = "(##) ####-####";
            
            String newValue = "";

            for (int i = 0; i < Value.Length; i++)
            {
                for (int j = 0; j <= 9; j++)
                    newValue += (Value.Substring(i, 1).Equals(j.ToString()) == true) ? Value.Substring(i, 1) : "";
            }

            Value = newValue;
            newValue = "";

            for (int i = 0, j = 0; i < Formato.Length; i++)
            {
                if (Formato.Substring(i, 1).Equals("#") == true)
                {
                    newValue += (Value.Length > j) ? Value.Substring(j, 1) : "";
                    j++;
                }
                else
                {
                    newValue += (Formato.Length > i) ? Formato.Substring(i, 1) : "";
                }
            }

            return newValue;
        }
    }
}
