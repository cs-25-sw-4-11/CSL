namespace CSL;

public record Calendar(Event[] Events)
{
    public static implicit operator Calendar(Event e) => new([e]);

    public static Calendar UnionOp(Calendar left, Calendar right)
    {
        return new Calendar(left.Events.Concat(right.Events).ToArray());
    }
}
