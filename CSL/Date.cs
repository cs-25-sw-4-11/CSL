namespace CSL;

public class Date
{
    public int Days { get; }
    public int Months { get; }
    public int Years { get; }

    public Date (int days, int months, int years)
    {
        Days = days;
        Months = months;
        Years = years;
    }
}