using CDL.Exceptions;
using CDL.TypeChecker;

namespace CDL.CLI;

using Antlr4.Runtime;
using System;
using System.IO;

class Program
{
    static int Main(string[] args)
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

        Console.WriteLine($"Input: {input}");
        Console.WriteLine();

        try
        {
            var compiler = new Compiler();
            var result = compiler.Compile(input);
            Console.WriteLine($"Result: {result}");
        }
        catch (CompilerException ce)
        {
            Console.WriteLine("Source code could not be evaluated, found the following error(s):");
            Console.WriteLine(ce.Message);
            return 1;
        }

        return 0;
    }
}