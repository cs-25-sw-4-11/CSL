namespace CSL;

public readonly struct Date(int days, int months, int years)
{
    public int Days { get; } = days;
    public int Months { get; } = months;
    public int Years { get; } = years;

    public override string ToString() => $"{Days}/{Months}/{Years}";
}
