using CSL.Exceptions;

namespace CSL;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

public class SubjectVisitor : CSLBaseVisitor<Subject?>
{
    public override Subject? VisitSubject(CSLParser.SubjectContext context)
    {
        if (context.SUBJECT() is null)
        {
            throw new InvalidLiteralCompilerException($"{nameof(Subject)}: Missing subject content");
        }

        string rawText = context.SUBJECT().GetText();

        if (rawText.Length < 2 || rawText[0] != '\'' || rawText[^1] != '\'')
        {
            throw new InvalidLiteralCompilerException($"{nameof(Subject)}: Subject must be properly single-quoted");
        }

        string text = rawText.Substring(1, rawText.Length - 2);

        if (text == "")
        {
            throw new InvalidLiteralCompilerException($"{nameof(Subject)}: Empty subject is not allowed");
        }

        return new Subject(text);
    }

    public override Subject? Visit(IParseTree tree)
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