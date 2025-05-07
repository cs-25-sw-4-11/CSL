namespace CSL;

public class Clock 
{
    public int Hours { get; }
    public int Minutes { get; }

    public Clock (int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
    }
}