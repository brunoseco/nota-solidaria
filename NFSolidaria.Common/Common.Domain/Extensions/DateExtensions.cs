using Common.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


public static class DateExtensions
{

    public static bool Intersects(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate, DateTime intersectingEndDate)
    {
        return (intersectingEndDate >= startDate && intersectingStartDate <= endDate);
    }

    public static bool UnderAge(this DateTime birthday)
    {
        var years = birthday.YearsOld();
        return years < 18;
    }

    /// <summary>
    /// Pega o dia da semana começando na segunda = 1, domingo = 7
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static EDayOfWeek GetDayOfWeek(this DateTime date)
    {
        var dia = (int)date.DayOfWeek;

        if (dia == 0) dia = 7;

        return (EDayOfWeek)dia;
    }

    public static bool ValidateDateStartToEnd(this DateTime startDate, DateTime endDate)
    {
        return endDate > startDate;
    }

    public static int YearsOld(this DateTime birthday)
    {

        int year = DateTime.Now.Year - birthday.Year;
        if (DateTime.Now.Month < birthday.Month || (DateTime.Now.Month == birthday.Month && DateTime.Now.Day < birthday.Day))
            year--;

        return year;

    }

    public static DateTime StartWeekDay(this DateTime data)
    {
        var weekDay = (int)data.DayOfWeek;
        return data.AddDays(-weekDay);

    }

    public static DateTime EndWeekDay(this DateTime data)
    {
        var weekDay = (int)data.DayOfWeek;
        var saturday = (int)DayOfWeek.Saturday;
        return data.AddDays((saturday - weekDay));
    }

    public static int MonthDays(this DateTime data)
    {
        int month = data.Month;
        int year = data.Year;
        return DateTime.DaysInMonth(year, month);
    }

    public static DateTime StartMonthDate(this DateTime data)
    {
        int dayToday = data.Day - 1;
        return data.AddDays(-dayToday);
    }

    public static DateTime EndMonthDate(this DateTime data)
    {
        int dayToday = data.Day;
        int lastDayOfMonth = data.MonthDays();
        int daysToAdd = lastDayOfMonth - dayToday;

        return data.AddDays(daysToAdd);
    }

    public static DateTime EndDay(this DateTime data)
    {
        return data.AddDays(1).AddMilliseconds(-1);
    }

    public static DateTime AddMonthInDate(this DateTime data, int months)
    {
        var dateBase = data.AddMonths(months);
        var lastDayInMonth = LastDayInMonth(dateBase);
        var dayBase = dateBase.Day;
        var day = dayBase > lastDayInMonth.Day ? lastDayInMonth.Day : dayBase;
        return new DateTime(dateBase.Year, dateBase.Month, day);
    }

    public static DateTime LastDayInMonth(DateTime dateBase)
    {
        if (dateBase.Month == 12)
            return new DateTime(dateBase.Year + 1, 2, 1).AddDays(-1);

        return new DateTime(dateBase.Year, dateBase.Month + 1, 1).AddDays(-1);
    }

    public static int ToMinutes(this DateTime data)
    {
        return (data.Hour * 60) + data.Minute;
    }

    public static int ToMinutes(this DateTime? data)
    {
        if (data.IsNull()) return 0;
        return (data.Value.Hour * 60) + data.Value.Minute;
    }


    public static DateTime[] DatesInInterval(this DateTime start, DateTime end, params DayOfWeek[] DayOfWeek)
    {
        var datesInterval = new List<DateTime>();
        var date = new DateTime(start.Year, start.Month, start.Day);

        while (date <= end)
        {
            if (DayOfWeek.Contains(date.DayOfWeek)) datesInterval.Add(date);
            date = date.AddDays(1);
        }

        return datesInterval.ToArray();
    }

    public static Expression<Func<TElement, bool>> IsSameDate<TElement>(Expression<Func<TElement, DateTime>> valueSelector, DateTime value)
    {
        ParameterExpression p = valueSelector.Parameters.Single();

        var antes = Expression.GreaterThanOrEqual(valueSelector.Body, Expression.Constant(value.Date, typeof(DateTime)));

        var despues = Expression.LessThan(valueSelector.Body, Expression.Constant(value.AddDays(1).Date, typeof(DateTime)));

        Expression body = Expression.And(antes, despues);

        return Expression.Lambda<Func<TElement, bool>>(body, p);
    }

    public static int DiasUteis(this DateTime inicio, DateTime termino, DateTime[] feriados)
    {
        var diasUteis = 0;

        for (var data = inicio; data <= termino; data = data.AddDays(1))
        {
            if (!feriados.Contains(data))
            {
                if (data.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasUteis++;
                }
            }
        }

        return diasUteis;
    }
}

