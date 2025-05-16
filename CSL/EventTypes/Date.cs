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
        return left.GetDateAsDuration() >= right; 
    }

    public static bool operator <=(Date left, Duration right)
    {
        return left.GetDateAsDuration() <= right; 
    }
}