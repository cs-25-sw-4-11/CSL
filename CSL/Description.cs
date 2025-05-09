namespace CSL;

public readonly struct Description(string text)
{
    public string Text { get; } = text;

    public override string ToString() => $"\"{Text}\"";
}
