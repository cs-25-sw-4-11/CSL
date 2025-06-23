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
        // Check if an argument was provided
        if (args.Length == 0)
        {
            Console.WriteLine("No input provided");
            return 1;
        }

        // Use the first command-line argument as input
        var inputFilename = args[0];

        if (!File.Exists(inputFilename))
        {
            Console.WriteLine($"Could not find file: {inputFilename}");
            return 1;
        }

        var input = File.ReadAllText(args[0]);

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