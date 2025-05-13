namespace CSL.SubTypes;

public readonly struct Description(string text)
{
    public string Text { get; } = text;

    public override string ToString() => $"\"{Text}\"";
    
    public static implicit operator Description(string text) => new (text);
}
