using CSL.Exceptions;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;

namespace CSL
{
    public static class GenericParser
    {
        public static TResult Parse<TResult, TVisitor, TContext>(
            string input,
            Func<TVisitor> visitorFactory,
            Func<CSLParser, TContext> parserRuleSelector,
            Func<TVisitor, TContext, TResult> visitorMethod,
            string entityName = "Input")
            where TContext : ParserRuleContext
        {
            var stream = CharStreams.fromString(input);
            var lexer = new CSLLexer(stream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new CSLLexerErrorListener());
                
            var tokens = new CommonTokenStream(lexer);
            var parser = new CSLParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new CSLParserErrorListener());
                
            var tree = parserRuleSelector(parser);
            var visitor = visitorFactory();
            return visitorMethod(visitor, tree);
        }
    }
    
    public static class ClockParser
    {
        public static Clock ParseClock(string input)
        {
            return GenericParser.Parse<Clock, ClockVisitor, CSLParser.ClockContext>(
                input,
                () => new ClockVisitor(),
                parser => parser.clock(),
                (visitor, tree) => visitor.VisitClock(tree),
                "Clock"
            );
        }
    }

    public static class DurationParser
    {
        public static Duration ParseDuration(string input)
        {
            return GenericParser.Parse<Duration, DurationVisitor, CSLParser.DurationContext>(
                input,
                () => new DurationVisitor(),
                parser => parser.duration(),
                (visitor, tree) => visitor.VisitDuration(tree),
                "Duration"
            );
        }
    }
}