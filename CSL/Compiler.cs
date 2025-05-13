namespace CSL;

public class Compiler
{
    public string Compile(string input)
    {
        var generator = new Generator();
        var calendar = generator.GenerateCalendar(input);

        return calendar.ToString();
    }
}