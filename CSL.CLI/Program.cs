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

        var compiler = new Compiler();
        var result = compiler.Compile(input);
        
        Console.WriteLine($"Input: {input}");
        Console.WriteLine($"Result: {result}");
    }
}