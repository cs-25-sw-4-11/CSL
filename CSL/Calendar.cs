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

    public static Calendar StrictlyBeforeOp(Calendar left, Calendar right)
    {
        foreach (Event e in left.Events) 
        {
            if (e.DateClock.HasValue)
            {
                throw new ArgumentException("Event has dateclock");
            }
        }
        foreach (Event e in right.Events)
        {
            
        }

        Event first = right.IsEvent() ? right.Events[0] : FindFirst(right);

        
    }

    private static Event FindFirst(Calendar calendar)
    {
        Event first = calendar.Events[0];
        
        foreach (Event e in calendar.Events)
        {
            if (IsEventEarlier(e, first))
            {
                first = e;
            }
        }
        
        return first;
    }

    private static bool IsEventEarlier(Event first, Event second)
    {
        if (first.DateClock.HasValue && second.DateClock.HasValue)
        {
            if (first.Date!.Value.CompareTo(second.Date!.Value) < 0)
            {
                return true;
            }
            else if (first.Date!.Value.CompareTo(second.Date!.Value) == 0)
            {
                return first.Clock!.Value.CompareTo(second.Clock!.Value) < 0;
            }
            return false;
        }
        
        if (first.Date.HasValue && second.Date.HasValue)
        {
            return first.Date.Value.CompareTo(second.Date.Value) < 0;
        }
        
        if (first.Clock.HasValue && second.Clock.HasValue)
        {
            return first.Clock.Value.CompareTo(second.Clock.Value) < 0;
        }

        return true;
    }

private static Event SetEventBeforeTarget(Event eventToModify, Event targetEvent)
{

    if (targetEvent.DateClock.HasValue)
    {
        DateClock targetTime = targetEvent.DateClock.Value;
        if (eventToModify.Duration.HasValue)
        {
            DateClock adjustedTime = targetTime - eventToModify.Duration.Value;
            return new Event(
                Subject: eventToModify.Subject,
                Date: adjustedTime.Date,
                Clock: adjustedTime.Clock,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description
            );
        }
        else
        {
            return new Event(
                Subject: eventToModify.Subject,
                Date: targetTime.Date,
                Clock: targetTime.Clock,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description
            );
        }
    }
    
    else if (targetEvent.Date.HasValue)
    {
        Date targetDate = targetEvent.Date.Value;
        if (eventToModify.Duration.HasValue && eventToModify.Duration.Value.Minutes % CSL.Duration.DayFactor == 0)
        {
            Date adjustedDate = targetDate - eventToModify.Duration.Value;
            return new Event(
                Subject: eventToModify.Subject,
                Date: adjustedDate,
                Clock: eventToModify.Clock,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description
            );
        }
        else
        {
            Date dayBefore = targetDate - new Duration(CSL.Duration.DayFactor);
            return new Event(
                Subject: eventToModify.Subject,
                Date: dayBefore,
                Clock: eventToModify.Clock,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description
            );
        }
    }
}