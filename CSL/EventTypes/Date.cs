namespace CSL.EventTypes;

public readonly struct Date(int days, int months, int years)
{
    public int Days { get; } = days;
    public int Months { get; } = months;
    public int Years { get; } = years;

    public override string ToString() => $"{Days}/{Months}/{Years}";

    public Duration GetDateAsDuration() => Duration.FromDays(Days)
                                           + Duration.FromMonths(Months)
                                           + Duration.FromYears(Years);


    public static Date operator +(Date left, Duration right)
    {
        Duration duration = left.GetDateAsDuration() +
                            right;

        return duration.GetDurationAsDate();
    }

    public static DateClock Plus(Date left, Duration right)
    {
        Duration duration = left.GetDateAsDuration() +
                            right;

        return duration.GetDurationAsDateClock();
    }
    public static Date operator -(Date left, Duration right)
    {
        Duration duration = left.GetDateAsDuration() - right;
        return duration.GetDurationAsDate();
    }
    public static DateClock Minus(Date left, Duration right)
    {
        Duration duration = left.GetDateAsDuration() - right;
        return duration.GetDurationAsDateClock();
    }

    public static bool operator >=(Date left, Duration right)
    {
        return left >= right.GetDurationAsDate();
    }
    public static bool operator <=(Date left, Duration right)
    {
        return left <= right.GetDurationAsDate();
    }

    public static bool operator >=(Date left, Date right)
    {
        if (left.Years > right.Years) return true;
        if (left.Years == right.Years)
        {
            if (left.Months > right.Months) return true;
            if (left.Months == right.Months)
            {
                if (left.Days >= right.Days) return true;
            }
        }
        return false;
    }

    public static bool operator <=(Date left, Date right)
    {
        if (left.Years < right.Years) return true;
        if (left.Years == right.Years)
        {
            if (left.Months < right.Months) return true;
            if (left.Months == right.Months)
            {
                if (left.Days <= right.Days) return true;
            }
        }
        return false;
    }
    public static bool operator ==(Date left, Date right)
    {
        if (left.Days != right.Days) return false;
        if (left.Months != right.Months) return false;
        if (left.Years != right.Years) return false;
        return true;
    }
    public static bool operator !=(Date left, Date right)
    {
        if (left.Days == right.Days) return false;
        if (left.Months == right.Months) return false;
        if (left.Years == right.Years) return false;
        return true;
    }
}