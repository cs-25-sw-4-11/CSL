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

    public static Calendar StrictlyBeforeOp(Calendar left, Calendar right)
    {
        foreach (Event e in left.Events) 
        {
            if (e.DateClock.HasValue)
            {
                throw new ArgumentException("Event has dateclock");
            }
        }

        Event first = right.IsEvent() ? right.Events[0] : FindFirst(right);
        List<Event> resultEvents = new List<Event>();
        
        foreach (Event e in left.Events)
        {
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
            // Compare Date values
            if (CompareDates(first.Date!.Value, second.Date!.Value) < 0)
            {
                return true;
            }
            else if (CompareDates(first.Date!.Value, second.Date!.Value) == 0)
            {
                // Compare Clock values
                return CompareClocks(first.Clock!.Value, second.Clock!.Value) < 0;
            }
            return false;
        }
        
        if (first.Date.HasValue && second.Date.HasValue)
        {
            // Use our comparison method
            return CompareDates(first.Date.Value, second.Date.Value) < 0;
        }
        
        if (first.Clock.HasValue && second.Clock.HasValue)
        {
            // Use our comparison method
            return CompareClocks(first.Clock.Value, second.Clock.Value) < 0;
        }

        return true;
    }

    // Helper method to compare dates
    private static int CompareDates(Date left, Date right)
    {
        if (left.Years != right.Years)
            return left.Years.CompareTo(right.Years);
        
        if (left.Months != right.Months)
            return left.Months.CompareTo(right.Months);
        
        return left.Days.CompareTo(right.Days);
    }

    // Helper method to compare clocks
    private static int CompareClocks(Clock left, Clock right)
    {
        if (left.Hours != right.Hours)
            return left.Hours.CompareTo(right.Hours);
        
        return left.Minutes.CompareTo(right.Minutes);
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
            Date dayBefore = targetDate - new Duration(CSL.Duration.DayFactor, 0);
            
            if (eventToModify.Duration.HasValue)
            {
                // Calculate end of day time based on duration
                int durationMinutes = eventToModify.Duration.Value.Minutes % CSL.Duration.DayFactor;
                
                // If the duration is in whole days or there's no remaining minutes component,
                // don't set a clock time
                if (durationMinutes == 0 || eventToModify.Duration.Value.Minutes % CSL.Duration.DayFactor == 0)
                {
                    return new Event(
                        Subject: eventToModify.Subject,
                        Date: dayBefore,
                        Clock: null,
                        Duration: eventToModify.Duration,
                        Description: eventToModify.Description
                    );
                }
                
                int durationHours = durationMinutes / 60;
                int durationRemainingMinutes = durationMinutes % 60;
                
                // Calculate the clock time for the day before (24 - duration hours)
                int hours = 24 - durationHours;
                int minutes = durationRemainingMinutes > 0 ? 60 - durationRemainingMinutes : 0;
                
                // Adjust hours if we needed to borrow from minutes
                if (durationRemainingMinutes > 0)
                    hours--;
                
                // Handle special case where hours could be 24, which is invalid
                if (hours == 24)
                    hours = 0;
                
                Clock newClock = new Clock(hours, minutes);
                
                return new Event(
                    Subject: eventToModify.Subject,
                    Date: dayBefore,
                    Clock: newClock,
                    Duration: eventToModify.Duration,
                    Description: eventToModify.Description
                );
            }
            else
            {
                // If no duration specified, just copy the date of the target event without setting a clock
                return new Event(
                    Subject: eventToModify.Subject,
                    Date: targetDate,
                    Clock: null,
                    Duration: eventToModify.Duration,
                    Description: eventToModify.Description
                );
            }
        }
        throw new ArgumentException("Invalid operation");
    }
}