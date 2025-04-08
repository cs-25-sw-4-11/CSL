using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
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
            // Testing Basic Tokens
            new object[]
            {
                "'Sub' + \"Des\" + Monday + 12/34/5678 + 10/10/2000 16:30 + 12:32 + 12min ",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.SUBJECT, "'Sub'"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.DESCRIPTION, "\"Des\""),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.DAYSOFWEEK, "Monday"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.DATE, "12/34/5678"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.DATE, "10/10/2000"),
                    new ExpectedToken(CSLLexer.INT, "16"),
                    new ExpectedToken(CSLLexer.COLON, ":"),
                    new ExpectedToken(CSLLexer.INT, "30"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "12"),
                    new ExpectedToken(CSLLexer.COLON, ":"),
                    new ExpectedToken(CSLLexer.INT, "32"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "12"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "min"),
                }
            },
            // Testing Operations
            new object[]
            {
                "'Subject' ~ 'Subject' ++ Complement 'Subject' + 'Subject' - 'Subject' in 'Subject' << 'Subject' >> 'Subject' < 'Subject' > 'Subject' * 'Subject' Intersect 'Subject' Union 'Subject'  ",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.THILDE, "~"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.COMPLEMENT, "Complement"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.MINUS, "-"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.IN, "in"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.SBEFORE, "<<"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.SAFTER, ">>"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.BEFORE, "<"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.AFTER, ">"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.STAR, "*"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.INTERSECTION, "Intersect"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.UNION, "Union"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),

                }
            },
            // Testing TimeUnits
            new object[]
            {
                "15sec + 3min + 1h + 4w + 0mth + 3y - 13 sec - 3 y - 4 w",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.INT, "15"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "sec"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "3"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "min"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "1"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "h"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "4"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "w"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "0"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "mth"),
                    new ExpectedToken(CSLLexer.PLUS, "+"),
                    new ExpectedToken(CSLLexer.INT, "3"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "y"),
                    new ExpectedToken(CSLLexer.MINUS, "-"),
                    new ExpectedToken(CSLLexer.INT, "13"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "sec"),
                    new ExpectedToken(CSLLexer.MINUS, "-"),
                    new ExpectedToken(CSLLexer.INT, "3"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "y"),
                    new ExpectedToken(CSLLexer.MINUS, "-"),
                    new ExpectedToken(CSLLexer.INT, "4"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "w"),
                }
            },
            /*
            new object[]
            {
                "15sec",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.INT, "15"),
                    new ExpectedToken(CSLLexer.TIMEUNITS, "sec"),                
                }
            },
            */
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


    public PrecedenceTestListener MakePrecedenceList(string input)
    {
        ICharStream stream = CharStreams.fromString(input);
        CSLLexer lexer = new CSLLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        CSLParser parser = new CSLParser(tokens);

        // Get the parse tree
        IParseTree tree = parser.expr();
        PrecedenceTestListener listener = new();
        ParseTreeWalker.Default.Walk(listener, tree);
        return listener;
    }
    [Theory]
    [InlineData("'a' + 'b' ~ 'c'")]
    [InlineData("'a' < 'b' ~ 'c'")]
    public void TestPrecedence(string input)
    {
        PrecedenceTestListener listener = MakePrecedenceList(input);
        var operations = listener.Operations;
        Console.WriteLine(string.Join(" -> ", operations));

        // Helper function to assert operator precedence
        void AssertPrecedence(string higherOp, string lowerOp)
        {
            int higherIndex = operations.IndexOf(higherOp);
            int lowerIndex = operations.IndexOf(lowerOp);

            if (higherIndex == -1)
            {
                Console.WriteLine($"{higherOp} not found in parse tree. Skipping test for {higherOp} vs {lowerOp}.");
            }
            else if (lowerIndex == -1)
            {
                Console.WriteLine($"{lowerOp} not found in parse tree. Skipping test for {higherOp} vs {lowerOp}.");
            }
            else
            {
                Assert.True(higherIndex < lowerIndex, $"{higherOp} should be processed before {lowerOp}");
            }
        }

        // Test 1: Ensure TildeOp has higher precedence than AddOp
        AssertPrecedence("TildeOp", "AddOp");

        // Test 2: Ensure ComplementOp has higher precedence than AddOp
        AssertPrecedence("ComplementOp", "AddOp");

        // Test 3: Ensure DoublePlusOp has higher precedence than AddOp
        AssertPrecedence("DoublePlusOp", "AddOp");

        // Test 4: Ensure AddOp has higher precedence than SubtractOp
        AssertPrecedence("AddOp", "SubtractOp");

        // Test 5: Ensure SubtractOp has higher precedence than InOp
        AssertPrecedence("SubtractOp", "InOp");

        // Test 6: Ensure InOp has higher precedence than SBEFORE
        AssertPrecedence("InOp", "SBEFORE");

        // Test 7: Ensure SBEFORE has higher precedence than SAFTER
        AssertPrecedence("SBEFORE", "SAFTER");

        // Test 8: Ensure SAFTER has higher precedence than BeforeOp
        AssertPrecedence("SAFTER", "BEFORE");

        // Test 9: Ensure BeforeOp has higher precedence than AfterOp
        AssertPrecedence("BEFORE", "AFTER");

        // Test 10: Ensure AddOp has higher precedence than RecursiveOp
        AssertPrecedence("AddOp", "RecursiveOp");

        // Test 11: Ensure RecursiveOp has higher precedence than IntersectOp
        AssertPrecedence("RecursiveOp", "IntersectOp");

        // Test 12: Ensure IntersectOp has higher precedence than UnionOp
        AssertPrecedence("IntersectOp", "UnionOp");

        // Test 13: Ensure TildeOp has higher precedence than ComplementOp
        AssertPrecedence("TildeOp", "ComplementOp");

        // Test 14: Ensure TildeOp has higher precedence than DoublePlusOp
        AssertPrecedence("TildeOp", "DoublePlusOp");

        // Test 15: Ensure ComplementOp has higher precedence than DoublePlusOp
        AssertPrecedence("ComplementOp", "DoublePlusOp");

        // Test 16: Ensure TildeOp has higher precedence than SubtractOp
        AssertPrecedence("TildeOp", "SubtractOp");

        // Test 17: Ensure TildeOp has higher precedence than InOp
        AssertPrecedence("TildeOp", "InOp");

        // Test 18: Ensure TildeOp has higher precedence than SBEFORE
        AssertPrecedence("TildeOp", "SBEFORE");

        // Test 19: Ensure TildeOp has higher precedence than SAFTER
        AssertPrecedence("TildeOp", "SAFTER");

        // Test 20: Ensure TildeOp has higher precedence than BeforeOp
        AssertPrecedence("TildeOp", "BEFORE");

        // Test 21: Ensure TildeOp has higher precedence than AfterOp
        AssertPrecedence("TildeOp", "AFTER");

        // Test 22: Ensure DoublePlusOp has higher precedence than SubtractOp
        AssertPrecedence("DoublePlusOp", "SubtractOp");

        // Test 23: Ensure DoublePlusOp has higher precedence than InOp
        AssertPrecedence("DoublePlusOp", "InOp");

        // Test 24: Ensure DoublePlusOp has higher precedence than SBEFORE
        AssertPrecedence("DoublePlusOp", "SBEFORE");

        // Test 25: Ensure DoublePlusOp has higher precedence than SAFTER
        AssertPrecedence("DoublePlusOp", "SAFTER");

        // Test 26: Ensure DoublePlusOp has higher precedence than BeforeOp
        AssertPrecedence("DoublePlusOp", "BEFORE");

        // Test 27: Ensure DoublePlusOp has higher precedence than AfterOp
        AssertPrecedence("DoublePlusOp", "AFTER");

        // Test 28: Ensure ComplementOp has higher precedence than SubtractOp
        AssertPrecedence("ComplementOp", "SubtractOp");

        // Test 29: Ensure ComplementOp has higher precedence than InOp
        AssertPrecedence("ComplementOp", "InOp");

        // Test 30: Ensure ComplementOp has higher precedence than SBEFORE
        AssertPrecedence("ComplementOp", "SBEFORE");

        // Test 31: Ensure ComplementOp has higher precedence than SAFTER
        AssertPrecedence("ComplementOp", "SAFTER");

        // Test 32: Ensure ComplementOp has higher precedence than BeforeOp
        AssertPrecedence("ComplementOp", "BEFORE");

        // Test 33: Ensure ComplementOp has higher precedence than AfterOp
        AssertPrecedence("ComplementOp", "AFTER");

        // Test 34: Ensure AddOp has higher precedence than InOp
        AssertPrecedence("AddOp", "InOp");

        // Test 35: Ensure AddOp has higher precedence than SBEFORE
        AssertPrecedence("AddOp", "SBEFORE");

        // Test 36: Ensure AddOp has higher precedence than SAFTER
        AssertPrecedence("AddOp", "SAFTER");

        // Test 37: Ensure AddOp has higher precedence than BeforeOp
        AssertPrecedence("AddOp", "BEFORE");

        // Test 38: Ensure AddOp has higher precedence than AfterOp
        AssertPrecedence("AddOp", "AFTER");

        // Test 39: Ensure InOp has higher precedence than SBEFORE
        AssertPrecedence("InOp", "SBEFORE");

        // Test 40: Ensure InOp has higher precedence than SAFTER
        AssertPrecedence("InOp", "SAFTER");

        // Test 41: Ensure InOp has higher precedence than BeforeOp
        AssertPrecedence("InOp", "BEFORE");

        // Test 42: Ensure InOp has higher precedence than AfterOp
        AssertPrecedence("InOp", "AFTER");

        // Test 43: Ensure SBEFORE has higher precedence than SAFTER
        AssertPrecedence("SBEFORE", "SAFTER");

        // Test 44: Ensure SBEFORE has higher precedence than BeforeOp
        AssertPrecedence("SBEFORE", "BEFORE");

        // Test 45: Ensure SBEFORE has higher precedence than AfterOp
        AssertPrecedence("SBEFORE", "AFTER");

        // Test 46: Ensure SAFTER has higher precedence than BeforeOp
        AssertPrecedence("SAFTER", "BEFORE");

        // Test 47: Ensure SAFTER has higher precedence than AfterOp
        AssertPrecedence("SAFTER", "AFTER");

        // Test 48: Ensure BeforeOp has higher precedence than AfterOp
        AssertPrecedence("BEFORE", "AFTER");

        // Test 49: Ensure RecursiveOp has higher precedence than IntersectOp
        AssertPrecedence("RecursiveOp", "IntersectOp");

        // Test 50: Ensure RecursiveOp has higher precedence than UnionOp
        AssertPrecedence("RecursiveOp", "UnionOp");

        // Test 51: Ensure IntersectOp has higher precedence than UnionOp
        AssertPrecedence("IntersectOp", "UnionOp");

    }
}