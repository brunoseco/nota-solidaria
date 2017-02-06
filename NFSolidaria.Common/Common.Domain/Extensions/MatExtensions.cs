using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public static class MathExstensions
{
    public static decimal AddPercentage(this decimal value, decimal? percentage)
    {
        return value.AddPercentage(percentage.IsNotNull() ? percentage.Value : 0);
    }
    public static decimal AddPercentage(this decimal value, decimal percentage)
    {
        return value + ((percentage / 100) * value);

    }
    public static decimal SubstractPercentage(this decimal value, decimal? percentage)
    {
        return value.SubstractPercentage(percentage.IsNotNull() ? percentage.Value : 0);
    }
    public static decimal SubstractPercentage(this decimal value, decimal percentage)
    {
        return value - ((percentage / 100) * value);

    }

    public static decimal DivisionTo(this decimal dividend, decimal divisor)
    {
        if (divisor == 0) return 0;
        return (dividend / divisor);

    }

    public static decimal CalcPercentage(this decimal value, decimal percentage)
    {
        return ((percentage / 100) * value);

    }

    public static decimal ToNegative(this decimal value)
    {
        return value < 0 ? value : value * -1;
    }

    public static decimal ToPositive(this decimal value)
    {
        return value < 0 ? value * -1 : value;
    }

    public static decimal CumulativeRate(this decimal value, IEnumerable<decimal> rates)
    {

        foreach (var rate in rates)
            value += ((value * (rate / 100)));

        return System.Math.Round(value, 2);

    }

    public static decimal CumulativeRateProRata(this decimal value, IEnumerable<KeyValuePair<DateTime, Decimal>> rates, DateTime deadline, DateTime paymentDate)
    {
        if (!rates.IsAny())
            return value;

        var dateIniRate = rates.Min(_ => _.Key);
        var dateEndRate = rates.Max(_ => _.Key).EndMonthDate();

        deadline = deadline.AddDays(1);
        var date = deadline > dateIniRate ? deadline : dateIniRate;
        var newDateEnd = paymentDate > dateEndRate ? dateEndRate : paymentDate;

        while (date <= newDateEnd)
        {
            var rate = rates.Where(_ => _.Key.Month == date.Month).Where(_ => _.Key.Year == date.Year).Select(_ => _.Value).SingleOrDefault();
            value += ((value * (rate / 100)));
            date = date.AddDays(1);
        }

        return System.Math.Round(value, 2);


    }

    public static string NumeroExtenso(this string number)
    {
        return Convert.ToDecimal(number).NumeroExtenso();
    }

    public static string NumeroExtenso(this decimal number)
    {
        int centavos;
        try
        {
            //se for igual a 0 retorna 0 reais
            if (number == 0)
                return "Zero Reais";
            //se passar do valor máximo retorna mensagem
            if (number > 999999999999)
                return "Valor máximo atingido";
            //Verifica a parte decimal, ou seja, os centavos
            centavos = (int)Decimal.Round((number - (long)number) * 100, MidpointRounding.ToEven);

            //Verifica apenas a parte inteira
            number = (long)number;
            //Caso existam centavos
            if (centavos > 0)
            {
                string strCent;
                if (centavos == 1)
                    strCent = " centavo";
                else
                    strCent = " centavos";

                //Caso seja 1 não coloca "Reais" mas sim "Real"
                if (number == 1)
                    return "Um Real e " + GetDecimal((byte)centavos) + strCent;
                //Caso o valor seja inferior a 1 Real
                else if (number == 0)
                    return GetDecimal((byte)centavos) + strCent;
                else
                    return GetInteger(number) + " Reais e " + GetDecimal((byte)centavos) + strCent;
            }
            else
            {
                //Caso seja 1 não coloca "Reais" mas sim "Real"
                if (number == 1)
                    return "Um Real";
                else
                    return GetInteger(number) + " Reais";

            }
        }
        catch (Exception)
        {
            return "";
        }
    }


    //Função auxiliar - Parte decimal a converter
    private static string GetDecimal(byte number)
    {
        try
        {
            if (number == 0)
                return "Zero Reais";
            else if (number >= 1 && number <= 19)
            {
                string[] strArray = {"Um", "Dois", "Três", "Quatro", "Cinco", "Seis", "Sete", "Oito", "Nove", "Dez",
                                    "Onze", "Doze","Treze", "Quatorze", "Quinze", "Dezesseis", "Dezessete", "Dezoito", "Dezenove"};
                return strArray[number - 1] + " ";
            }
            else if (number >= 20 && number <= 99)
            {
                string[] strArray = { "Vinte", "Trinta", "Quarenta", "Cinquenta", "Sessenta", "Setenta", "Oitenta", "Noventa" };

                if (number % 10 == 0)
                    return strArray[number / 10 - 2] + " ";
                else
                    return strArray[number / 10 - 2] + " e " + GetDecimal((byte)(number % 10)) + " ";
            }
            else
                return string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }   // fim do metodo GetDecimal


    //Função auxiliar - Parte inteira a converter
    private static string GetInteger(Decimal decnumber)
    {
        try
        {
            long number = (long)decnumber;
            if (number < 0)
                return "-" + GetInteger((long)-number);
            else if (number == 0)
                return string.Empty;
            else if (number >= 1 && number <= 19)
            {
                string[] strArray = {"Um", "Dois", "Três", "Quatro", "Cinco", "Seis", "Sete", "Oito", "Nove", "Dez",
                                    "Onze", "Doze", "Treze", "Quatorze", "Quinze", "Dezesseis", "Dezessete", "Dezoito","Dezenove"};
                return strArray[(long)number - 1] + " ";
            }
            else if (number >= 20 && number <= 99)
            {
                string[] strArray = { "Vinte", "Trinta", "Quarenta", "Cinquenta", "Sessenta", "Setenta", "Oitenta", "Noventa" };

                if (number % 10 == 0)
                    return strArray[(long)number / 10 - 2];
                else
                    return strArray[(long)number / 10 - 2] + " e " + GetInteger(number % 10);
            }
            else if (number == 100)
                return "Cem";
            else if (number >= 101 && number <= 999)
            {
                string[] strArray = { "Cento", "Duzentos", "Trezentos", "Quatrocentos", "Quinhentos", "Seiscentos", "Setecentos", "Oitocentos", "Novecentos" };
                if (number % 100 == 0)
                    return strArray[(long)number / 100 - 1] + " ";
                else
                    return strArray[(long)number / 100 - 1] + " e " + GetInteger(number % 100);
            }
            else if (number >= 1000 && number <= 1999)
            {
                if (number % 1000 == 0)
                    return "Mil";
                else if (number % 1000 <= 100)
                    return "Mil e " + GetInteger(number % 1000);
                else
                    return "Mil, " + GetInteger(number % 1000);
            }
            else if (number >= 2000 && number <= 999999)
            {
                if (number % 1000 == 0)
                    return GetInteger((decimal)number / 1000) + "Mil";
                else if (number % 1000 <= 100)
                    return GetInteger((decimal)number / 1000) + "Mil e " + GetInteger(number % 1000);
                else
                    return GetInteger((decimal)number / 1000) + "Mil, " + GetInteger(number % 1000);
            }
            else if (number >= 1000000 && number <= 1999999)
            {
                if (number % 1000000 == 0)
                    return "Um Milhão";
                else if (number % 1000000 <= 100)
                    return GetInteger((decimal)number / 1000000) + "Milhão e " + GetInteger(number % 1000000);
                else
                    return GetInteger((decimal)number / 1000000) + "Milhão, " + GetInteger(number % 1000000);
            }
            else if (number >= 2000000 && number <= 999999999)
            {
                if (number % 1000000 == 0)
                    return GetInteger((decimal)number / 1000000) + " Milhões";
                else if (number % 1000000 <= 100)
                    return GetInteger((decimal)number / 1000000) + "Milhões e " + GetInteger(number % 1000000);
                else
                    return GetInteger((decimal)number / 1000000) + "Milhões, " + GetInteger(number % 1000000);
            }
            else if (number >= 1000000000 && number <= 1999999999)
            {
                if (number % 1000000000 == 0)
                    return "Um Bilhão";
                else if (number % 1000000000 <= 100)
                    return GetInteger((decimal)number / 1000000000) + "Bilhão e " + GetInteger(number % 1000000000);
                else
                    return GetInteger((decimal)number / 1000000000) + "Bilhão, " + GetInteger(number % 1000000000);
            }
            else
            {
                if (number % 1000000000 == 0)
                    return GetInteger((decimal)number / 1000000000) + " Bilhões";
                else if (number % 1000000000 <= 100)
                    return GetInteger((decimal)number / 1000000000) + "Bilhões e " + GetInteger(number % 1000000000);
                else
                    return GetInteger((decimal)number / 1000000000) + "Bilhões, " + GetInteger(number % 1000000000);
            }
        }

        catch (Exception)
        {
            return string.Empty;
        }
    }


}

