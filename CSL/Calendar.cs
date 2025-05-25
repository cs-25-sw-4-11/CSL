namespace CSL;
using EventTypes;
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
    
    public Event this[int index] => Events[index];

    public Event? Event => IsEvent() ? Events[0] : null;

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

    public static Calendar HideOperator(Calendar c)
    {
        return new Calendar(c.Events.Select(e => Event.HideOperator(e)).ToArray());
    }
    
    public static Calendar RecurrenceOperator(Calendar c, Duration interval)
    {
        return new Calendar(c.Events.Select(ev => CSL.Event.RecurrenceOperator(ev, interval)).ToArray());
    }

    public static Calendar StrictlyBeforeOp(Calendar left, Calendar right)
    {
        Event first = right.IsEvent() ? right.Events[0] : FindFirst(right);
        List<Event> resultEvents = new List<Event>();

        foreach (Event e in left.Events)
        {
            if (e.DateClock.HasValue)
            {
                throw new ArgumentException("Event has dateclock");
            }

            Event modifiedEvent = SetEventBeforeTarget(e, first);
            resultEvents.Add(modifiedEvent);
        }

        return new Calendar(resultEvents.Union(right.Events).ToArray());
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
            return first.DateClock.Value <= second.DateClock.Value;
        }

        if (first.Date.HasValue && second.Date.HasValue)
        {
            return first.Date.Value <= second.Date.Value;
        }

        if (first.Clock.HasValue && second.Clock.HasValue)
        {
            return first.Clock.Value <= second.Clock.Value;
        }

        return true;
    }

    private static Event SetEventBeforeTarget(Event eventToModify, Event targetEvent)
    {
        if (targetEvent.Date is null && targetEvent.Clock is null)
        {
            throw new ArgumentException($"Target event must have either {nameof(Date)} or {nameof(Clock)}");
        }

        if (targetEvent.DateClock.HasValue)
        {
            DateClock targetTime = targetEvent.DateClock.Value;
            DateClock adjustedTime = eventToModify.Duration.HasValue
                ? targetTime - eventToModify.Duration.Value
                : targetTime;

            return new Event(
                Subject: eventToModify.Subject,
                dateClock: adjustedTime,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description,
                Hidden: eventToModify.Hidden
            );
        }

        if (targetEvent.Clock.HasValue)
        {
            Clock targetTime = targetEvent.Clock.Value;
            Clock adjustedTime = eventToModify.Duration.HasValue
                ? targetTime - eventToModify.Duration.Value
                : targetTime;

            return new Event(
                Subject: eventToModify.Subject,
                Clock: adjustedTime,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description,
                Hidden: eventToModify.Hidden
            );
        }

        Date targetDate = targetEvent.Date!.Value;
        if (eventToModify.Duration is null)
        {
            return new Event(
                Subject: eventToModify.Subject,
                Date: targetDate,
                Description: eventToModify.Description,
                Hidden: eventToModify.Hidden
            );
        }

        Duration duration = eventToModify.Duration.Value;
        if (duration.Minutes % Duration.DayFactor == 0)
        {
            Date newDate = targetDate - duration;
            return new Event(
                Subject: eventToModify.Subject,
                Date: newDate,
                Duration: eventToModify.Duration,
                Description: eventToModify.Description,
                Hidden: eventToModify.Hidden
            );
        }

        DateClock targetDateClock = new DateClock(targetDate, new Clock(0, 0));
        DateClock adjustedDateClock = targetDateClock - duration;
        return new Event(
            Subject: eventToModify.Subject,
            dateClock: adjustedDateClock,
            Duration: eventToModify.Duration,
            Description: eventToModify.Description,
            Hidden: eventToModify.Hidden
        );
    }
}