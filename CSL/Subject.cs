namespace CSL;

public readonly struct Subject(string text)
{
    public string Text { get; } = text;

    public override string ToString() => $"'{Text}'";
}
