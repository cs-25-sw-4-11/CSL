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
        var events = new List<Event>();

        foreach (Event e in c.Events)
        {
            events.Add(Event.HideOperator(e));
        }

        return new Calendar(events.ToArray());
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
            Duration secondDuration = second.Date!.Value.GetDateAsDuration() + second.Clock!.Value.GetClockAsDuration();
            return first.DateClock <= secondDuration;
        }

        if (first.Date.HasValue && second.Date.HasValue)
        {
            Duration secondDuration = second.Date.Value.GetDateAsDuration();
            return first.Date.Value <= secondDuration;
        }

        if (first.Clock.HasValue && second.Clock.HasValue)
        {
            Duration secondDuration = second.Clock.Value.GetClockAsDuration();
            return first.Clock.Value <= secondDuration;
        }

        return true;
    }

    private static Event SetEventBeforeTarget(Event eventToModify, Event targetEvent)
    {
        if (targetEvent.DateClock is null && targetEvent.Date is null)
        {
            throw new ArgumentException($"Target event must have either {nameof(DateClock)} or {nameof(Date)}");
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
                Description: eventToModify.Description
            );
        }

        Date targetDate = targetEvent.Date!.Value;
        if (eventToModify.Duration is null)
        {
            return new Event(
                Subject: eventToModify.Subject,
                Date: targetDate,
                Description: eventToModify.Description
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
                Description: eventToModify.Description
            );
        }

        DateClock targetDateClock = new DateClock(targetDate, new Clock(0, 0));
        DateClock adjustedDateClock = targetDateClock - duration;
        return new Event(
            Subject: eventToModify.Subject,
            dateClock: adjustedDateClock,
            Duration: eventToModify.Duration,
            Description: eventToModify.Description
        );
    }
}