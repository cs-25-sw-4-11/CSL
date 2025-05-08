using CSL.TypeChecker;

namespace CSL.CLI;

using Antlr4.Runtime;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string input;

        // Check if an argument was provided
        if (args.Length > 0)
        {
            // Use the first command-line argument as input
            input = args[0];
        }
        else
        {
            // Default input if no arguments provided
            input = "11:00 ++ 11/11/2011 ++ 'Caspers kagespisning'";
            Console.WriteLine("No input provided, using default: " + input);
        }

        var stream = CharStreams.fromString(input);

        var lexer = new CSLLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();

        // Type checker
        var typeVisitor = new TypeCheckerVisitor();
        typeVisitor.Visit(tree);
        
        // Calendar visitor
        var visitor = new EventVisitor();
        var result = visitor.Visit(tree);

        Console.WriteLine($"Input: {input}");
        Console.WriteLine($"Result: {result}");
    }
}