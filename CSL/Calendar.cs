namespace CSL;

public record Calendar(Event[] Events)
{
    public static implicit operator Calendar(Event e) => new([e]);

    public override string ToString() => "{" + string.Join(", ", Events.Select(e => e.ToString())) + "}";

    public static Calendar UnionOp(Calendar left, Calendar right)
    {
        return new Calendar(left.Events.Concat(right.Events).ToArray());
    }

    public static Calendar ConcatOperator(Calendar left, Event right)
    {
        IList<Event> newCalendar = new List<Event>();
        
        foreach (var e in left.Events)
        {
            newCalendar.Add(Event.ConcatOperator(e, right));
        }

        return new(newCalendar.ToArray());
    }
}
