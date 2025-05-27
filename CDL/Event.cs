using System.Text;
using Antlr4.Runtime.Atn;

namespace CDL;

using EventTypes;

public record Event(
    Subject? Subject = null,
    Date? Date = null,
    Clock? Clock = null,
    Duration? Duration = null,
    Description? Description = null,
    bool? Hidden = null,
    Duration? RepeatInterval = null)
{
    public DateClock? DateClock
    {
        get
        {
            if (Date is null || Clock is null)
            {
                return null;
            }

            return new(Date.Value, Clock.Value);
        }
    }

    public Event(DateClock dateClock,
        Subject? Subject = null,
        Duration? Duration = null,
        Description? Description = null,
        bool? Hidden = null,
        Duration? RepeatInterval = null)
        : this(
            Subject: Subject,
            Date: dateClock.Date,
            Clock: dateClock.Clock,
            Duration: Duration,
            Description: Description,
            Hidden: Hidden,
            RepeatInterval: RepeatInterval)
    {
    }

    /// <summary>
    /// Bases of the current Event, and applies the new attributes.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="date"></param>
    /// <param name="clock"></param>
    /// <param name="duration"></param>
    /// <param name="description"></param>
    /// <param name="hidden"></param>
    /// <param name="repeatInterval"></param>
    /// <returns></returns>
    public Event With(Subject? subject = null,
        Date? date = null,
        Clock? clock = null,
        Duration? duration = null,
        Description? description = null,
        bool? hidden = null,
        Duration? repeatInterval = null)
    {
        return new Event(
            Subject: subject ?? this.Subject,
            Date: date ?? this.Date,
            Clock: clock ?? this.Clock,
            Duration: duration ?? this.Duration,
            Description: description ?? this.Description,
            Hidden: hidden ?? this.Hidden,
            RepeatInterval: repeatInterval ?? this.RepeatInterval
        );
    }

    /// <summary>
    /// Bases of the current Event, and applies the new attributes.
    /// </summary>
    /// <param name="dateClock"></param>
    /// <param name="subject"></param>
    /// <param name="duration"></param>
    /// <param name="description"></param>
    /// <param name="hidden"></param>
    /// <param name="repeatInterval"></param>
    /// <returns></returns>
    public Event With(DateClock dateClock,
        Subject? subject = null,
        Duration? duration = null,
        Description? description = null,
        bool? hidden = null,
        Duration? repeatInterval = null)
    {
        return new Event(
            dateClock,
            Subject: subject ?? this.Subject,
            Duration: duration ?? this.Duration,
            Description: description ?? this.Description,
            Hidden: hidden ?? this.Hidden,
            RepeatInterval: repeatInterval ?? this.RepeatInterval
        );
    }

    /// <summary>
    /// Spreads <code>e</code> and applies With.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public Event With(Event e)
    {
        return new Event(
            Subject: e.Subject ?? this.Subject,
            Date: e.Date ?? this.Date,
            Clock: e.Clock ?? this.Clock,
            Duration: e.Duration ?? this.Duration,
            Description: e.Description ?? this.Description,
            Hidden: e.Hidden ?? this.Hidden,
            RepeatInterval: e.RepeatInterval ?? this.RepeatInterval
        );
    }
    
    /// <summary>
    /// Tries to get the datetime for an event.
    /// If an event has a clock, then it gets added as well.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>Whether the datetime was able to be constructed.</returns>
    public bool TryGetDateTime(out DateTime dateTime)
    {
        if (Date is null)
        {
            dateTime = default;
            return false;
        }

        if (Clock.HasValue)
        {
            dateTime = new DateTime(Date.Value.Years,
                Date.Value.Months,
                Date.Value.Days,
                Clock.Value.Hours,
                Clock.Value.Minutes,
                0);
            return true;
        }

        dateTime = new DateTime(
            Date.Value.Years,
            Date.Value.Months,
            Date.Value.Days);
        return true;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('(');

        if (Subject.HasValue)
        {
            sb.Append($"subj:{Subject.Value}, ");
        }

        if (Date.HasValue)
        {
            sb.Append($"date:{Date.Value}, ");
        }

        if (Clock.HasValue)
        {
            sb.Append($"clock:{Clock.Value}, ");
        }

        if (Duration.HasValue)
        {
            sb.Append($"duration:{Duration.Value}, ");
        }

        if (Description.HasValue)
        {
            sb.Append($"description:{Description.Value}, ");
        }

        if (Hidden.HasValue)
        {
            sb.Append($"Hidden:{Hidden.Value}, ");
        }
        sb.Append(")");
        return sb.ToString();
    }

    public static Event ConcatOperator(Event left, Event right)
    {
        if (left.Subject.HasValue && right.Subject.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Subject)}");
        }

        if (left.Date.HasValue && right.Date.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Date)}");
        }

        if (left.Clock.HasValue && right.Clock.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Clock)}");
        }

        if (left.Duration.HasValue && right.Duration.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Duration)}");
        }

        if (left.Description.HasValue && right.Description.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Description)}");
        }

        return right.With(left);
    }

    public static Event AddOperator(Event left, Event right)
    {
        Event otherOperand;
        Event firstOperand;
        // checks if one of the events is only duration and puts it as the first operand
        if (left is { Duration: not null, Subject: null, Description: null, Clock: null, Date: null })
        {
            firstOperand = left;
            otherOperand = right;
        }
        else if (right is { Duration: not null, Subject: null, Description: null, Clock: null, Date: null })
        {
            firstOperand = right;
            otherOperand = left;
        }
        else
        {
            throw new ArgumentException($"Missing expression with only {nameof(Duration)}");
        }

        // checks if the other operand is a duration, date, dateclock or clock and calculates accordingly 
        if (otherOperand is { Duration: not null, DateClock: null })
        {
            return otherOperand.With(
                duration: firstOperand.Duration + otherOperand.Duration
            );
        }

        if (otherOperand.Duration is null && otherOperand.Date.HasValue)
        {
            if (otherOperand.Duration is null && otherOperand.DateClock.HasValue)
            {
                var dateclock = otherOperand.DateClock.Value;
                var duration = firstOperand.Duration;
                var result = dateclock + duration;

                return otherOperand.With(
                    result.Value
                );
            }

            if (firstOperand.Duration.Value.Minutes % EventTypes.Duration.DayFactor == 0)
            {
                return otherOperand.With(
                    date: otherOperand.Date + firstOperand.Duration
                );
            }

            var date = otherOperand.Date.Value;
            var dur = firstOperand.Duration.Value;
            var result1 = EventTypes.Date.Plus(date, dur);

            return otherOperand.With(
                result1
            );
        }

        if (otherOperand.Duration is null && otherOperand.Clock.HasValue)
        {
            return otherOperand.With(
                clock: otherOperand.Clock + firstOperand.Duration
            );
        }

        throw new ArgumentException(
            $"Missing expression with either {nameof(Duration)} or {nameof(Date)} and {nameof(Clock)} ");
    }

    public static Event SubOperator(Event left, Event right)
    {
        if (right.Duration is null)
        {
            throw new ArgumentException(
                $"Missing expression with {nameof(Duration)}");
        }
        if (left.Duration is not null && right.Date is not null || right.Clock is not null)
        {
            throw new ArgumentException(
                $"Can not have both {nameof(Duration)} and ({nameof(Date)} or {nameof(Clock)})");
        }

        if (left.DateClock.HasValue)
        {
            if (left.DateClock.Value >= right.Duration)
            {
                return left.With(left.DateClock.Value - right.Duration.Value);
            }
        }

        if (left.Date.HasValue)
        {
            if (right.Duration.Value.Minutes % EventTypes.Duration.DayFactor == 0)
            {
                if (left.Date.Value >= right.Duration)
                {
                    return left.With(date: left.Date.Value - right.Duration.Value);
                }
            }
            else
            {
                return left.With(EventTypes.Date.Minus(left.Date.Value, right.Duration.Value));
            }
        }
        if (left.Clock.HasValue)
        {
            return left.With(
                clock: left.Clock.Value - right.Duration.Value
            );
        }

        if (left.Duration is not null)
        {
            if (left.Duration >= right.Duration)
            {
                // Doesn't call .value here, an error?
                return left.With(duration: left.Duration - right.Duration);
            }
        }

        throw new ArgumentException(
            $"Missing expression with either {nameof(Duration)} or {nameof(Date)} and {nameof(Clock)} ");
    }

    public static Event HideOperator(Event e)
    {
        return e.With(hidden: true);
    }

    public static Event RecurrenceOperator(Event ev, Duration interval)
    {
        return ev.With(repeatInterval: interval);
    }
    
    public static Event TildeOperator(Event left, Event right)
    {
        // Handle DateClock ~ DateClock
        if (left.DateClock.HasValue && right.DateClock.HasValue)
        {
            DateClock leftDateClock = left.DateClock.Value;
            DateClock rightDateClock = right.DateClock.Value;
            Duration dateClockResult = EventTypes.DateClock.TildeOp(leftDateClock, rightDateClock);
            return new Event(Duration: dateClockResult);
        }
        
        if (left.Date.HasValue && right.Clock.HasValue && !left.Clock.HasValue && !right.Date.HasValue)
        {
            throw new InvalidOperationException("Invalid combination of event properties for Range operation");
        }
        if (left.Clock.HasValue && !left.Date.HasValue && !right.Clock.HasValue && right.Date.HasValue)
        {
            throw new InvalidOperationException("Invalid combination of event properties for Range operation");
        }
        if (left.Clock.HasValue && !left.Date.HasValue && right.DateClock.HasValue)
        {
            throw new InvalidOperationException("Invalid combination of event properties for Range operation");            
        }
        if (left.DateClock.HasValue && right.Clock.HasValue && !right.Date.HasValue)
        {
            throw new InvalidOperationException("Invalid combination of event properties for Range operation");    
        }
        if (left.Date.HasValue && left.Clock.HasValue && right.Clock.HasValue && !right.Date.HasValue)
        {
            throw new InvalidOperationException("Invalid combination of event properties for Range operation");
        }
        if (left.Clock.HasValue && !left.Date.HasValue && right.Date.HasValue && right.Clock.HasValue)
        {
            throw new InvalidOperationException("Invalid combination of event properties for Range operation");
        }

        // Handle DateClock ~ Date
        if (left.DateClock.HasValue && right.Date.HasValue)
        {
            DateClock leftDateClock = left.DateClock.Value;
            Date rightDate = right.Date.Value;
            Duration result = EventTypes.DateClock.TildeOp(leftDateClock, rightDate);
            return new Event(Duration: result);
        }

        // Handle Date ~ DateClock
        if (left.Date.HasValue && right.DateClock.HasValue)
        {
            Date leftDate = left.Date.Value;
            DateClock rightDateClock = right.DateClock.Value;
            Duration result = EventTypes.DateClock.TildeOp(leftDate, rightDateClock);
            return new Event(Duration: result);
        }

        // Handle Date ~ Date
            if (left.Date.HasValue && right.Date.HasValue)
            {
                Date leftDate = left.Date.Value;
                Date rightDate = right.Date.Value;
                Duration dateResult = EventTypes.Date.TildeOp(leftDate, rightDate);
                return new Event(Duration: dateResult);
            }

        // Handle Clock ~ Clock
        if (left.Clock.HasValue && right.Clock.HasValue && !right.Date.HasValue)
        {
            Clock leftClock = left.Clock.Value;
            Clock rightClock = right.Clock.Value;
            Duration clockResult = EventTypes.Clock.TildeOp(leftClock, rightClock);
            return new Event(Duration: clockResult);
        }

        // If we get here, something is wrong with the events
        throw new InvalidOperationException("Invalid combination of event properties for Range operation");
    }

    /*Duration.GetDurationAsDate*/
    /// <summary>
    /// Returns the event of a singleton Calendar.
    /// </summary>
    /// <param name="c">Singleton Calendar</param>
    /// <returns>The event of the calendar</returns>
    /// <exception cref="InvalidCastException">Throws an exception if Calendar doesn't contain exactly one event</exception>
    public static explicit operator Event(Calendar c)
    {
        if (c.Events.Length != 1)
        {
            throw new InvalidCastException($"Can't cast Calendar to event, because Calendar contains '{c.Events.Length}' events");
        }

        return c.Events[0];
    }
}