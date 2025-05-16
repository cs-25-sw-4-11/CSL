using CSL.Exceptions;

namespace CSL.EventTypes;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

public class DescriptionVisitor : CSLBaseVisitor<Description?>
{
    public override Description? VisitDescription(CSLParser.DescriptionContext context)
    {
        if (context.DESCRIPTION() is null)
        {
            throw new InvalidLiteralCompilerException($"{nameof(Description)}: Missing description content");
        }

        string rawText = context.DESCRIPTION().GetText();

        if (rawText.Length < 2 || rawText[0] != '"' || rawText[^1] != '"')
        {
            throw new InvalidLiteralCompilerException($"{nameof(Description)}: Description must be properly quoted");
        }

        string text = rawText.Substring(1, rawText.Length - 2);

        if (text == "")
        {
            throw new InvalidLiteralCompilerException($"{nameof(Description)}: Empty description is not allowed");
        }

        return new Description(text);
    }

    public override Description? Visit(IParseTree tree)
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