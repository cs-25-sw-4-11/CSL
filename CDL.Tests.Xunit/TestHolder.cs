namespace CDL.Tests.Xunit;

public class TestHolder{
    
    public string Input {get; set;}
    public List<ExpectedToken> ExpectedTokens {get; set;}
    public TestHolder(string input, List<ExpectedToken> expectedTokens) {
        Input = input;
        ExpectedTokens = expectedTokens;   
    }


}