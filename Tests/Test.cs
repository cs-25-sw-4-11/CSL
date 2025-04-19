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
                "'Subject' ~ 'Subject' ++ !'Subject' + 'Subject' - 'Subject' in 'Subject' << 'Subject' >> 'Subject' < 'Subject' > 'Subject' * 'Subject' && 'Subject' || 'Subject'  ",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.THILDE, "~"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.COMPLEMENT, "!"),
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
                    new ExpectedToken(CSLLexer.INTERSECTION, "&&"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Subject'"),
                    new ExpectedToken(CSLLexer.UNION, "||"),
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
            // Wrong grammar no use of ' inside subject
            new object[]
            {
                "'Day's'",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.SUBJECT, "'Day'"),
                    new ExpectedToken(CSLLexer.IDENTIFIER, "s"),
                    new ExpectedToken(CSLLexer.Eof, "<EOF>")                
                }
            },
            // Identifier and Stat test
            new object[]
            {
                "WorkWeek = 7:00~16:00 ++ Monday ++ Tuesday ++ Wednesday ++ Thursday ++ Friday;",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.IDENTIFIER, "WorkWeek"),
                    new ExpectedToken(CSLLexer.EQUAL, "="),
                    new ExpectedToken(CSLLexer.INT, "7"),
                    new ExpectedToken(CSLLexer.COLON, ":"),
                    new ExpectedToken(CSLLexer.INT, "00"),
                    new ExpectedToken(CSLLexer.THILDE, "~"),
                    new ExpectedToken(CSLLexer.INT, "16"),
                    new ExpectedToken(CSLLexer.COLON, ":"),
                    new ExpectedToken(CSLLexer.INT, "00"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.DAYSOFWEEK, "Monday"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.DAYSOFWEEK, "Tuesday"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.DAYSOFWEEK, "Wednesday"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.DAYSOFWEEK, "Thursday"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.DAYSOFWEEK, "Friday"),
                    new ExpectedToken(CSLLexer.SEMICOLON, ";")                  
                }
            },
            // Testing Identifiers names
            new object[]
            {
                "ADWdsadjlawWADJ897931987gjfsjf8fhe8_4356347856uwifeWDA ++ _Succes ++ 0fail ++ _0Suc ++ _0",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.IDENTIFIER, "ADWdsadjlawWADJ897931987gjfsjf8fhe8_4356347856uwifeWDA"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.IDENTIFIER, "_Succes"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    // Opserve the user indented to have a id called 0fail, but that is not allowed in the grammer and is seen as a int and id
                    new ExpectedToken(CSLLexer.INT, "0"),
                    new ExpectedToken(CSLLexer.IDENTIFIER, "fail"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.IDENTIFIER, "_0Suc"),
                    new ExpectedToken(CSLLexer.PLUSPLUS, "++"),
                    new ExpectedToken(CSLLexer.IDENTIFIER, "_0"),                         
                }
            },
            // Comment test
            new object[]
            {
                "// This is a comment should not get a token \n 'Token' \n /* Comment \n Still comment \n Surpise still comment */\n 'Token'",
                new List<ExpectedToken>
                {
                    new ExpectedToken(CSLLexer.SUBJECT, "'Token'"),
                    new ExpectedToken(CSLLexer.SUBJECT, "'Token'"),                
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

    // Helper method to parse an expression and return the parse tree
    private static CSLParser.ExprContext ParseExpression(string input)
    {
        ICharStream stream = CharStreams.fromString(input);
        CSLLexer lexer = new CSLLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        CSLParser parser = new CSLParser(tokens);
        return parser.expr();
    }

    [Theory]
    // Combinations with ParenExpr
    [InlineData("(a) ~ b", "ParenExpr", "TildeOp")]
    [InlineData("(a) ++ b", "ParenExpr", "DoublePlusOp")]
    [InlineData("(a) + b", "ParenExpr", "AddOp")]
    [InlineData("(a) - b", "ParenExpr", "SubtractOp")]
    [InlineData("(a) in b", "ParenExpr", "InOp")]
    [InlineData("(a) << b", "ParenExpr", "StrictlyBeforeOp")]
    [InlineData("(a) >> b", "ParenExpr", "StrictlyAfterOp")]
    [InlineData("(a) < b", "ParenExpr", "BeforeOp")]
    [InlineData("(a) > b", "ParenExpr", "AfterOp")]
    [InlineData("(a) * b", "ParenExpr", "RecursiveOp")]
    [InlineData("(a) && b", "ParenExpr", "IntersectOp")]
    [InlineData("(a) || b", "ParenExpr", "UnionOp")]
    [InlineData("a ~ (b)", "ParenExpr", "TildeOp")]
    [InlineData("a ++ (b)", "ParenExpr", "DoublePlusOp")]
    [InlineData("a + (b)", "ParenExpr", "AddOp")]
    [InlineData("a - (b)", "ParenExpr", "SubtractOp")]
    [InlineData("a in (b)", "ParenExpr", "InOp")]
    [InlineData("a << (b)", "ParenExpr", "StrictlyBeforeOp")]
    [InlineData("a >> (b)", "ParenExpr", "StrictlyAfterOp")]
    [InlineData("a < (b)", "ParenExpr", "BeforeOp")]
    [InlineData("a > (b)", "ParenExpr", "AfterOp")]
    [InlineData("a * (b)", "ParenExpr", "RecursiveOp")]
    [InlineData("a && (b)", "ParenExpr", "IntersectOp")]
    [InlineData("a || (b)", "ParenExpr", "UnionOp")]
    // Combinations with HideExpr
    [InlineData("[a] ~ b", "HideExpr", "TildeOp")]
    [InlineData("[a] ++ b", "HideExpr", "DoublePlusOp")]
    [InlineData("[a] + b", "HideExpr", "AddOp")]
    [InlineData("[a] - b", "HideExpr", "SubtractOp")]
    [InlineData("[a] in b", "HideExpr", "InOp")]
    [InlineData("[a] << b", "HideExpr", "StrictlyBeforeOp")]
    [InlineData("[a] >> b", "HideExpr", "StrictlyAfterOp")]
    [InlineData("[a] < b", "HideExpr", "BeforeOp")]
    [InlineData("[a] > b", "HideExpr", "AfterOp")]
    [InlineData("[a] * b", "HideExpr", "RecursiveOp")]
    [InlineData("[a] && b", "HideExpr", "IntersectOp")]
    [InlineData("[a] || b", "HideExpr", "UnionOp")]
    [InlineData("a ~ [b]", "HideExpr", "TildeOp")]
    [InlineData("a ++ [b]", "HideExpr", "DoublePlusOp")]
    [InlineData("a + [b]", "HideExpr", "AddOp")]
    [InlineData("a - [b]",  "HideExpr", "SubtractOp")]
    [InlineData("a in [b]", "HideExpr", "InOp")]
    [InlineData("a << [b]", "HideExpr", "StrictlyBeforeOp")]
    [InlineData("a >> [b]", "HideExpr", "StrictlyAfterOp")]
    [InlineData("a < [b]", "HideExpr", "BeforeOp")]
    [InlineData("a > [b]", "HideExpr", "AfterOp")]
    [InlineData("a * [b]", "HideExpr", "RecursiveOp")]
    [InlineData("a && [b]", "HideExpr", "IntersectOp")]
    [InlineData("a || [b]", "HideExpr", "UnionOp")]
    // Combinations with ComplementOp
    [InlineData("!a ~ b", "TildeOp", "ComplementOp")]
    [InlineData("!a ++ b", "ComplementOp", "DoublePlusOp")]
    [InlineData("!a + b", "ComplementOp", "AddOp")]
    [InlineData("!a - b", "ComplementOp", "SubtractOp")]
    [InlineData("!a in b", "ComplementOp", "InOp")]
    [InlineData("!a << b", "ComplementOp", "StrictlyBeforeOp")]
    [InlineData("!a >> b", "ComplementOp", "StrictlyAfterOp")]
    [InlineData("!a < b", "ComplementOp", "BeforeOp")]
    [InlineData("!a > b", "ComplementOp", "AfterOp")]
    [InlineData("!a * b", "ComplementOp", "RecursiveOp")]
    [InlineData("!a && b", "ComplementOp", "IntersectOp")]
    [InlineData("!a || b", "ComplementOp", "UnionOp")]
    [InlineData("!(a ~ b)", "ParenExpr", "ComplementOp")]
    // TildeOp combinations
    [InlineData("a ~ b ++ c", "TildeOp", "DoublePlusOp")]
    [InlineData("a ~ b + c", "TildeOp", "AddOp")]
    [InlineData("a ~ b - c", "TildeOp", "SubtractOp")]
    [InlineData("a ~ b in c", "TildeOp", "InOp")]
    [InlineData("a ~ b << c", "TildeOp", "StrictlyBeforeOp")]
    [InlineData("a ~ b >> c", "TildeOp", "StrictlyAfterOp")]
    [InlineData("a ~ b < c", "TildeOp", "BeforeOp")]
    [InlineData("a ~ b > c", "TildeOp", "AfterOp")]
    [InlineData("a ~ b * c", "TildeOp", "RecursiveOp")]
    [InlineData("a ~ b && c", "TildeOp", "IntersectOp")]
    [InlineData("a ~ b || c", "TildeOp", "UnionOp")]
    // DoublePlusOp combinations
    [InlineData("a ++ b ~ c", "TildeOp", "DoublePlusOp")]
    [InlineData("a ++ b + c", "DoublePlusOp", "AddOp")]
    [InlineData("a ++ b - c", "DoublePlusOp", "SubtractOp")]
    [InlineData("a ++ b in c", "DoublePlusOp", "InOp")]
    [InlineData("a ++ b << c", "DoublePlusOp", "StrictlyBeforeOp")]
    [InlineData("a ++ b >> c", "DoublePlusOp", "StrictlyAfterOp")]
    [InlineData("a ++ b < c", "DoublePlusOp", "BeforeOp")]
    [InlineData("a ++ b > c", "DoublePlusOp", "AfterOp")]
    [InlineData("a ++ b * c", "DoublePlusOp", "RecursiveOp")]
    [InlineData("a ++ b && c", "DoublePlusOp", "IntersectOp")]
    [InlineData("a ++ b || c", "DoublePlusOp", "UnionOp")]
    // AddOp combinations
    [InlineData("a + b ~ c", "TildeOp", "AddOp")]
    [InlineData("a + b ++ c", "DoublePlusOp", "AddOp")]
    [InlineData("a + b - c", "AddOp", "SubtractOp")]
    [InlineData("a + b in c", "AddOp", "InOp")]
    [InlineData("a + b << c", "AddOp", "StrictlyBeforeOp")]
    [InlineData("a + b >> c", "AddOp", "StrictlyAfterOp")]
    [InlineData("a + b < c", "AddOp", "BeforeOp")]
    [InlineData("a + b > c", "AddOp", "AfterOp")]
    [InlineData("a + b * c", "AddOp", "RecursiveOp")]
    [InlineData("a + b && c", "AddOp", "IntersectOp")]
    [InlineData("a + b || c", "AddOp", "UnionOp")]
    // SubtractOp combinations
    [InlineData("a - b ~ c", "TildeOp", "SubtractOp")]
    [InlineData("a - b ++ c", "DoublePlusOp", "SubtractOp")]
    [InlineData("a - b + c", "AddOp", "SubtractOp")]
    [InlineData("a - b in c", "SubtractOp", "InOp")]
    [InlineData("a - b << c", "SubtractOp", "StrictlyBeforeOp")]
    [InlineData("a - b >> c", "SubtractOp", "StrictlyAfterOp")]
    [InlineData("a - b < c", "SubtractOp", "BeforeOp")]
    [InlineData("a - b > c", "SubtractOp", "AfterOp")]
    [InlineData("a - b * c", "SubtractOp", "RecursiveOp")]
    [InlineData("a - b && c", "SubtractOp", "IntersectOp")]
    [InlineData("a - b || c", "SubtractOp", "UnionOp")]
    // InOp combinations
    [InlineData("a in b ~ c", "TildeOp", "InOp")]
    [InlineData("a in b ++ c", "DoublePlusOp", "InOp")]
    [InlineData("a in b + c", "AddOp", "InOp" )]
    [InlineData("a in b - c", "SubtractOp", "InOp")]
    [InlineData("a in b << c", "InOp", "StrictlyBeforeOp")]
    [InlineData("a in b >> c", "InOp", "StrictlyAfterOp")]
    [InlineData("a in b < c", "InOp", "BeforeOp")]
    [InlineData("a in b > c", "InOp", "AfterOp")]
    [InlineData("a in b * c", "InOp", "RecursiveOp")]
    [InlineData("a in b && c", "InOp", "IntersectOp")]
    [InlineData("a in b || c", "InOp", "UnionOp")]
    // StrictlyBeforeOp combinations
    [InlineData("a << b ~ c", "TildeOp", "StrictlyBeforeOp")]
    [InlineData("a << b ++ c", "DoublePlusOp", "StrictlyBeforeOp")]
    [InlineData("a << b + c", "AddOp", "StrictlyBeforeOp")]
    [InlineData("a << b - c", "SubtractOp", "StrictlyBeforeOp")]
    [InlineData("a << b in c", "InOp", "StrictlyBeforeOp")]
    [InlineData("a << b >> c", "StrictlyBeforeOp", "StrictlyAfterOp")]
    [InlineData("a << b < c", "StrictlyBeforeOp", "BeforeOp")]
    [InlineData("a << b > c", "StrictlyBeforeOp", "AfterOp")]
    [InlineData("a << b * c", "StrictlyBeforeOp", "RecursiveOp")]
    [InlineData("a << b && c", "StrictlyBeforeOp", "IntersectOp")]
    [InlineData("a << b || c", "StrictlyBeforeOp", "UnionOp")]
    // StrictlyAfterOp combinations
    [InlineData("a >> b ~ c", "TildeOp", "StrictlyAfterOp")]
    [InlineData("a >> b ++ c", "DoublePlusOp", "StrictlyAfterOp")]
    [InlineData("a >> b + c", "AddOp", "StrictlyAfterOp")]
    [InlineData("a >> b - c", "SubtractOp", "StrictlyAfterOp")]
    [InlineData("a >> b in c", "InOp", "StrictlyAfterOp")]
    [InlineData("a >> b << c", "StrictlyBeforeOp", "StrictlyAfterOp")]
    [InlineData("a >> b < c", "StrictlyAfterOp", "BeforeOp")]
    [InlineData("a >> b > c", "StrictlyAfterOp", "AfterOp")]
    [InlineData("a >> b * c", "StrictlyAfterOp", "RecursiveOp")]
    [InlineData("a >> b && c", "StrictlyAfterOp", "IntersectOp")]
    [InlineData("a >> b || c", "StrictlyAfterOp", "UnionOp")]
    // BeforeOp combinations
    [InlineData("a < b ~ c", "TildeOp", "BeforeOp")]
    [InlineData("a < b ++ c", "DoublePlusOp", "BeforeOp")]
    [InlineData("a < b + c", "AddOp", "BeforeOp")]
    [InlineData("a < b - c", "SubtractOp", "BeforeOp")]
    [InlineData("a < b in c", "InOp", "BeforeOp")]
    [InlineData("a < b << c", "StrictlyBeforeOp", "BeforeOp")]
    [InlineData("a < b >> c", "StrictlyAfterOp", "BeforeOp")]
    [InlineData("a < b > c", "BeforeOp", "AfterOp")]
    [InlineData("a < b * c", "BeforeOp", "RecursiveOp")]
    [InlineData("a < b && c", "BeforeOp", "IntersectOp")]
    [InlineData("a < b || c", "BeforeOp", "UnionOp")]
    // AfterOp combinations
    [InlineData("a > b ~ c",  "TildeOp", "AfterOp")]
    [InlineData("a > b ++ c",  "DoublePlusOp", "AfterOp")]
    [InlineData("a > b + c",  "AddOp", "AfterOp")]
    [InlineData("a > b - c",  "SubtractOp", "AfterOp")]
    [InlineData("a > b in c",  "InOp", "AfterOp")]
    [InlineData("a > b << c",  "StrictlyBeforeOp", "AfterOp")]
    [InlineData("a > b >> c",  "StrictlyAfterOp", "AfterOp")]
    [InlineData("a > b < c",  "BeforeOp", "AfterOp")]
    [InlineData("a > b * c", "AfterOp", "RecursiveOp")]
    [InlineData("a > b && c", "AfterOp", "IntersectOp")]
    [InlineData("a > b || c", "AfterOp", "UnionOp")]
    // RecursiveOp combinations
    [InlineData("a * b ~ c",  "TildeOp", "RecursiveOp")]
    [InlineData("a * b ++ c",  "DoublePlusOp", "RecursiveOp")]
    [InlineData("a * b + c",  "AddOp", "RecursiveOp")]
    [InlineData("a * b - c",  "SubtractOp", "RecursiveOp")]
    [InlineData("a * b in c",  "InOp", "RecursiveOp")]
    [InlineData("a * b << c",  "StrictlyBeforeOp", "RecursiveOp")]
    [InlineData("a * b >> c",  "StrictlyAfterOp", "RecursiveOp")]
    [InlineData("a * b < c",  "BeforeOp", "RecursiveOp")]
    [InlineData("a * b > c",  "AfterOp", "RecursiveOp")]
    [InlineData("a * b && c", "RecursiveOp", "IntersectOp")]
    [InlineData("a * b || c", "RecursiveOp", "UnionOp")]
    // IntersectOp combinations
    [InlineData("a && b ~ c",  "TildeOp", "IntersectOp")]
    [InlineData("a && b ++ c",  "DoublePlusOp", "IntersectOp")]
    [InlineData("a && b + c",  "AddOp", "IntersectOp")]
    [InlineData("a && b - c",  "SubtractOp", "IntersectOp")]
    [InlineData("a && b in c",  "InOp", "IntersectOp")]
    [InlineData("a && b << c",  "StrictlyBeforeOp", "IntersectOp")]
    [InlineData("a && b >> c",  "StrictlyAfterOp", "IntersectOp")]
    [InlineData("a && b < c",  "BeforeOp", "IntersectOp")]
    [InlineData("a && b > c",  "AfterOp", "IntersectOp")]
    [InlineData("a && b * c",  "RecursiveOp", "IntersectOp")]
    [InlineData("a && b || c", "IntersectOp", "UnionOp")]
    // UnionOp combinations
    [InlineData("a || b ~ c",  "TildeOp", "UnionOp")]
    [InlineData("a || b ++ c",  "DoublePlusOp", "UnionOp")]
    [InlineData("a || b + c",  "AddOp", "UnionOp")]
    [InlineData("a || b - c",  "SubtractOp", "UnionOp")]
    [InlineData("a || b in c",  "InOp", "UnionOp")]
    [InlineData("a || b << c",  "StrictlyBeforeOp", "UnionOp")]
    [InlineData("a || b >> c",  "StrictlyAfterOp", "UnionOp")]
    [InlineData("a || b < c",  "BeforeOp", "UnionOp")]
    [InlineData("a || b > c", "AfterOp", "UnionOp")]
    [InlineData("a || b * c", "RecursiveOp", "UnionOp")]
    [InlineData("a || b && c", "IntersectOp", "UnionOp")]
    // Additional complex combinations
    [InlineData("(a + b) * c", "ParenExpr", "RecursiveOp")]
    [InlineData("a + (b * c)", "ParenExpr", "AddOp")]
    [InlineData("[a + b] && c", "HideExpr", "IntersectOp")]
    [InlineData("a || [b && c]", "HideExpr", "UnionOp" )]

    public void TestOperatorPrecedence(string expression, string higherPrecedenceOp, string lowerPrecedenceOp)
    {
        // Parse the expression
        var exprContext = ParseExpression(expression);
        
        // Convert context type name to operation name
        string GetOperationName(CSLParser.ExprContext ctx)
        {
            string typeName = ctx.GetType().Name;
            // Remove "Context" suffix if it exists
            if (typeName.EndsWith("Context"))
                typeName = typeName.Substring(0, typeName.Length - 7);
            return typeName;
        }

        // Extract all the operations from the tree
        List<string> operations = ExtractOperations(exprContext);
        Console.WriteLine($"Expression: {expression}, Operations: {string.Join(", ", operations)}");
        
        // The higher precedence op should be evaluated first (appear earlier in the list)
        int highIndex = operations.IndexOf(higherPrecedenceOp);
        int lowIndex = operations.IndexOf(lowerPrecedenceOp);
        
        Assert.True(highIndex >= 0, $"Higher precedence operation '{higherPrecedenceOp}' not found");
        Assert.True(lowIndex >= 0, $"Lower precedence operation '{lowerPrecedenceOp}' not found");
        Assert.True(highIndex < lowIndex, 
            $"'{higherPrecedenceOp}' should be evaluated before '{lowerPrecedenceOp}' but was found at index {highIndex} vs {lowIndex}");
    }
    
    // Extract operations in execution order (depth-first post-order traversal)
    private List<string> ExtractOperations(CSLParser.ExprContext context)
    {
        var operations = new List<string>();
        ExtractOperationsRecursive(context, operations);
        return operations;
    }
    
    private void ExtractOperationsRecursive(CSLParser.ExprContext context, List<string> operations)
    {
        // Skip literal and identifier expressions
        if (context is CSLParser.LiteralExprContext || context is CSLParser.IdentifierExprContext)
            return;
            
        // For unary operators, process the operand first
        if (context is CSLParser.ComplementOpContext)
        {
            var complementOp = (CSLParser.ComplementOpContext)context;
            ExtractOperationsRecursive(complementOp.expr(), operations);
            operations.Add("ComplementOp");
            return;
        }
        
        // For binary operators, process both operands first
        // This simulates the execution order - operands are evaluated before the operator
        var property = context.GetType().GetMethod("expr", new Type[] { typeof(int) });
        if (property != null)
        {
            var left = (CSLParser.ExprContext)property.Invoke(context, new object[] { 0 });
            if (left != null)
                ExtractOperationsRecursive(left, operations);
                
            if (property.Invoke(context, new object[] { 1 }) is CSLParser.ExprContext right)
                ExtractOperationsRecursive(right, operations);
        }
        
        // Add this operation to the list after processing its operands
        string typeName = context.GetType().Name;
        if (typeName.EndsWith("Context"))
            typeName = typeName.Substring(0, typeName.Length - 7);
            
        operations.Add(typeName);
    }
}