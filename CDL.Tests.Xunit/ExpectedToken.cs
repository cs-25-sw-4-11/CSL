namespace CDL.Tests.Xunit;

public class ExpectedToken
{
    public int Type { get; set; }
    public string Text { get; set; }
    
    public ExpectedToken(int type, string text)
    {
        Type = type;
        Text = text;
    }
}