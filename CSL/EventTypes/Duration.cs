namespace CSL.EventTypes;

public readonly struct Duration(int minutes, int months)
{
    public int Minutes { get; } = minutes;

    public int Months { get; } = months;

    public override string ToString() => $"{Minutes} min:{Months} months";

    public static Duration FromMinutes(int minutes) => new Duration(minutes, 0);

    public static Duration FromHours(int hours) => FromMinutes(60 * hours);

    public static Duration FromDays(int days) => FromHours(24 * days);

    public static Duration FromWeeks(int weeks) => FromDays(7 * weeks);

    public static Duration FromMonths(int months) => new Duration(0, months);

    public static Duration FromYears(int years) => FromMonths(12 * years);

    public const int MinuteFactor = 1;

    public const int HourFactor = 60 * MinuteFactor;

    public const int DayFactor = 24 * HourFactor;

    public const int WeekFactor = 7 * DayFactor;

    public const int MonthFactor = 1;

    public const int YearFactor = 12 * MonthFactor;

    public static Duration operator +(Duration left, Duration right)
    {
        return new Duration(
            minutes: left.Minutes + right.Minutes,
            months: left.Months + right.Months
        );
    }

    public static Duration operator -(Duration left, Duration right)
    {
        return new Duration(
            minutes: left.Minutes - right.Minutes,
            months: left.Months - right.Months
        );
    }
    public static bool operator >=(Duration left, Duration right)
    {

        return left.GetDurationAsDateTime() >= right.GetDurationAsDateTime();
    }

    public static bool operator <=(Duration left, Duration right)
    {
        return left.GetDurationAsDateTime() <= right.GetDurationAsDateTime();
    }

    public DateTime GetDurationAsDateTime()
    {
        DateTime referenceDate = new DateTime(1, 1, 1);
        return referenceDate.AddMonths(Months).AddMinutes(Minutes);
    }

    public DateClock GetDurationAsDateClock()
    {
        return new DateClock(
            date: GetDurationAsDate(),
            clock: GetDurationAsClock()
        );
    }

    public Date GetDurationAsDate()
    {
        int days = Minutes / DayFactor;
        int months = Months % YearFactor;
        int years = Months / YearFactor;

        // Handle zero or negative months
        if (months <= 0)
        {
            int yearAdjustment = (Math.Abs(months) + 11) / 12;
            months += 12 * yearAdjustment;
            years -= yearAdjustment;
        }

        // Handle zero or negative days
        while (days <= 0)
        {
            months -= 1;

            // Handle month underflow
            if (months <= 0)
            {
                months = 12;
                years -= 1;
            }

            int daysInPreviousMonth = months switch
            {
                1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
                4 or 6 or 9 or 11 => 30,
                2 => DateTime.IsLeapYear(years) ? 29 : 28,
                _ => throw new InvalidOperationException("Invalid month")
            };

            days += daysInPreviousMonth;
        }

        return new Date(
            days: days,
            months: months,
            years: years
        );
    }

    public Clock GetDurationAsClock()
    {
        int minutes = this.Minutes;
        minutes = ((minutes % DayFactor) + DayFactor) % DayFactor;
        return new Clock(
            hours: minutes / HourFactor,
            minutes: minutes % HourFactor
        );
    }
    public static Duration Zero => new(0, 0);
}