using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit;



public class LexerTests
{
    public static IEnumerable<object[]> TokenTestData =>
        new List<object[]>
        {
            new object[]
            {
                "'Sub' + \"Des\"",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.SUBJECT, "'Sub'"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.DESCRIPTION, "\"Des\""),
                }
            },
            new object[]
            {
                "12/12/1212 > 10:11",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.DATE, "12/12/1212"),
                    new ExpectedToken(CSLLexer.AFTER, ">"),
                    new ExpectedToken(CSLLexer.CLOCK, "10:11"),
                }
            },
            // More test here ...
        };

    [Theory]
    [MemberData(nameof(TokenTestData))]
    public void TestLexer(string input, List<ExpectedToken> expectedTokens)
    {
        // Create an input stream from the input string
        ICharStream stream = CharStreams.fromString(input);
        
        // Create the lexer
        CSLLexer lexer = new CSLLexer(stream);
        
        // Track if all tokens match expectations
        bool allTokensCorrect = true;
        int tokenIndex = 0;
        List<string> errors = new List<string>();
        
        // Get all tokens and compare with expected
        IToken token;
        do
        {
            
            token = lexer.NextToken();
            
            // Skip EOF comparison unless explicitly expected
            if (token.Type == Lexer.Eof && tokenIndex >= expectedTokens.Count)
            {
                break;
            }
            
            // Check if we have more tokens than expected
            if (tokenIndex >= expectedTokens.Count)
            {
                errors.Add($"ERROR: More tokens than expected. Extra token: {token.Type} : '{token.Text}'");
                allTokensCorrect = false;
                break;
            }
            
            // Get expected token info
            ExpectedToken expected = expectedTokens[tokenIndex];
            
            // Compare token type and text
            if (token.Type != expected.Type || token.Text != expected.Text)
            {
                errors.Add($"ERROR: Token mismatch at position {tokenIndex}");
                errors.Add($"  Expected: {expected.Type} : '{expected.Text}'");
                errors.Add($"  Got: {token.Type} : '{token.Text}'");
                allTokensCorrect = false;
            }
            

            tokenIndex++;
        } while (token.Type != Lexer.Eof);
        
        // Check if we expected more tokens
        if (tokenIndex < expectedTokens.Count)
        {
            errors.Add($"ERROR: Fewer tokens than expected. Missing {expectedTokens.Count - tokenIndex} tokens.");
            allTokensCorrect = false;
        }
        
        // Use XUnit assertion
        Assert.True(allTokensCorrect, string.Join(Environment.NewLine, errors));
    }

}