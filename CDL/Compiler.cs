using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;

namespace CDL;

public class Compiler
{
    public string Compile(string input)
    {
        var generator = new Generator();
        var calendar = generator.GenerateCalendar(input);
        
        calendar.AddTimeZone(new VTimeZone("Europe/Copenhagen")); // TZ should be added
        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        return serializedCalendar;
    }
}