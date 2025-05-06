using CSL.Exceptions;

namespace CSL;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;

public class DateVisitor : CSLBaseVisitor<Date>
{
    public override Date VisitLiteral(CSLParser.LiteralContext context)
    {
        if (context.DATE() is null)
        {
            throw new InvalidLiteralCompilerException($"{nameof(Date)}: Missing date content");
        }

        string rawText = context.DATE().GetText();

        string[] parts = rawText.Split('/');

        if (parts.Length != 3)
        {
            throw new InvalidLiteralCompilerException($"{nameof(Date)}: Invalid format, expected 'd/m/yyyy'");
        }

        if (!int.TryParse(parts[0], out int day))
        {
            throw new InvalidLiteralCompilerException($"{nameof(Date)}: Invalid day: '{parts[0]}'");
        }

        if (!int.TryParse(parts[1], out int month))
        {
            throw new InvalidLiteralCompilerException($"{nameof(Date)}: Invalid month: '{parts[1]}'");
        }

        if (!int.TryParse(parts[2], out int year))
        {
            throw new InvalidLiteralCompilerException($"{nameof(Date)}: Invalid year: '{parts[2]}'");
        }

        try
        {
            // Validate using built-in .NET DateTime but return our custom type
            var _ = new DateTime(year, month, day);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidLiteralCompilerException($"{nameof(Date)}: Invalid calendar date: {rawText}");
        }

        return new Date(day, month, year);
    }

    public override Date Visit(IParseTree tree)
    {
        try
        {
            return base.Visit(tree);
        }
        catch (CompilerException)
        {
            throw;
        }
    }
}
