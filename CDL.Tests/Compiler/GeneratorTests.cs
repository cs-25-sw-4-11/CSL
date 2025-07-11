using System.Collections;
using CDL.EventTypes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace CDL.Tests;

public class GeneratorTests
{
    public static IEnumerable GeneratorTestCases
    {
        get
        {
            var ical = new Ical.Net.Calendar();
            var boosterEvent = new CalendarEvent
            {
                // If Name property is used, it MUST be RFC 5545 compliant
                Summary = "Bund en booster",
                Description = "",
                Start = new CalDateTime(new DateTime(2025, 6, 25, 17, 50, 0)),
                End = new CalDateTime(new DateTime(2025, 6, 25, 17, 55, 0)),
            };
        
            var hygiejneEvent = new CalendarEvent
            {
                // If Name property is used, it MUST be RFC 5545 compliant
                Summary = "Vask hænder",
                Description = "",
                Start = new CalDateTime(new DateTime(2025, 6, 25, 17, 55, 0)),
                End = new CalDateTime(new DateTime(2025, 6, 25, 18, 0, 0)),
            };
        
            var aftensmadEvent = new CalendarEvent
            {
                // If Name property is used, it MUST be RFC 5545 compliant
                Summary = "Aftensmad",
                Description = "",
                Start = new CalDateTime(new DateTime(2025, 6, 25, 18, 0, 0)),
                End = new CalDateTime(new DateTime(2025, 6, 25, 19, 0, 0)),
            };

            ical.Events.Add(boosterEvent);
            ical.Events.Add(hygiejneEvent);
            ical.Events.Add(aftensmadEvent);
            
            yield return new TestCaseData(new Calendar([
                new Event(Subject: "Bund en booster", Date: new Date(25, 6, 2025), Clock: new Clock(17, 50), Duration: Duration.FromMinutes(5)),
                new Event(Subject: "Vask hænder", Date: new Date(25, 6, 2025), new Clock(17, 55), Duration: Duration.FromMinutes(5)),
                new Event(Subject: "Aftensmad", new Date(25, 6, 2025), new Clock(18, 0), Duration: Duration.FromHours(1))
            ]), ical);
        }
    }

    public static IEnumerable CaseStudy2TestCase
    {
        get
        {
            var ical = new Ical.Net.Calendar();
            
            var medsEvent = new CalendarEvent
            {
                Summary = "Take Medicine",
                Description = "Take the blue pill, Neo",
                Start = new CalDateTime(new DateTime(2025, 4, 11, 17, 25, 0)),
                End = new CalDateTime(new DateTime(2025, 4, 11, 17, 30, 0)),
            };
            
            var dinnerEvent = new CalendarEvent
            {
                Summary = "Dinner",
                Description = "",
                Start = new CalDateTime(new DateTime(2025, 4, 11, 18, 0, 0)),
                End = new CalDateTime(new DateTime(2025, 4, 11, 18, 30, 0)),
            };

            var rrule = new RecurrencePattern(FrequencyType.Minutely, 24 * 60);
            medsEvent.RecurrenceRules = new List<RecurrencePattern> { rrule };
            dinnerEvent.RecurrenceRules = new List<RecurrencePattern> { rrule };
            
            ical.Events.Add(medsEvent);
            ical.Events.Add(dinnerEvent);

            yield return new TestCaseData(new Calendar([]), ical);
        }
    }
    
    [TestCaseSource(nameof(GeneratorTestCases))]
    [TestCaseSource(nameof(CaseStudy2TestCase))]
    public void CompareCalendars(Calendar calendar, Ical.Net.Calendar expectedCalendar)
    {
        expectedCalendar.AddTimeZone(new VTimeZone("Europe/Copenhagen")); // TZ should be added
        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(expectedCalendar);

        Console.WriteLine(serializedCalendar);
    }
}