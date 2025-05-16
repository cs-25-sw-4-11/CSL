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

    public bool IsEvent()
    {
        return Events.Length == 1;
    }

    public static Calendar AddOperator(Calendar left, Event right)
    {
        var events = new List<Event>();

        foreach (Event e in left.Events)
        {
            events.Add(Event.AddOperator(e, right));
        }

        return new Calendar(events.ToArray());
    }

    public static Calendar SubOperator(Calendar left, Event right)
    {
        var events = new List<Event>();

        foreach (Event e in left.Events)
        {
            events.Add(Event.SubOperator(e, right));
        }

        return new Calendar(events.ToArray());
    }
}