namespace CSL;

public record Calendar(Event[] Events)
{
    public static implicit operator Calendar(Event e) => new([e]);
}
