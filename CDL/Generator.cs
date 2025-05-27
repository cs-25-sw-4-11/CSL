using Antlr4.Runtime;
using CDL.EventTypes;
using CDL.TypeChecker;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace CDL;

public class Generator
{
    public static readonly Event DefaultEvent = new Event(Subject: "Unnamed Event", Description: "", Duration: Duration.FromHours(1));
    
    public Ical.Net.Calendar GenerateCalendar(string input)
    {
        var stream = CharStreams.fromString(input);

        var lexer = new CSLLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        
        // Type checker
        var typeVisitor = new TypeCheckerVisitor();
        typeVisitor.Visit(tree);
        
        // Calendar visitor
        var visitor = new CalendarVisitor();
        var result = visitor.Visit(tree);

        return ConvertCalendar(result);
    }

    Ical.Net.Calendar ConvertCalendar(Calendar calendar)
    {
        var ical = new Ical.Net.Calendar();

        foreach (var calEvent in calendar.Events)
        {
            if (calEvent.Hidden ?? false)
            {
                continue;
            }
            
            if (!calEvent.TryGetDateTime(out DateTime dateTime))
            {
                // If there's a default value, then use that instead.
                if (!DefaultEvent.TryGetDateTime(out dateTime))
                {
                    continue;
                }
            }

            var subject = calEvent.Subject ?? DefaultEvent.Subject!.Value;
            var description = calEvent.Description ?? DefaultEvent.Description!.Value;
            var duration = calEvent.Duration ?? DefaultEvent.Duration!.Value;
            
            var calendarEvent = new CalendarEvent
            {
                // If Name property is used, it MUST be RFC 5545 compliant
                Summary = subject,
                Description = description, // optional
                Start = new CalDateTime(dateTime),
                End = new CalDateTime(dateTime.AddMonths(duration.Months).AddMinutes(duration.Minutes)),
            };
            
            if (calEvent.RepeatInterval.HasValue)
            {
                RecurrencePattern? rrule = null;
                if (calEvent.RepeatInterval.Value.Minutes != 0)
                {
                    rrule = new RecurrencePattern(FrequencyType.Minutely, calEvent.RepeatInterval.Value.Minutes);
                }else if (calEvent.RepeatInterval.Value.Months != 0)
                {
                    rrule = new RecurrencePattern(FrequencyType.Monthly, calEvent.RepeatInterval.Value.Months);
                }

                if (rrule is not null)
                {
                    calendarEvent.RecurrenceRules = new List<RecurrencePattern>() { rrule };
                }
            }
            
            ical.Events.Add(calendarEvent);
        }
        
        return ical;
    }
}