namespace CSL.SubTypes;

public readonly struct Subject(string text)
{
    public string Text { get; } = text;

    public override string ToString() => $"'{Text}'";
    
    public static implicit operator Subject(string text) => new (text);
    
    public static implicit operator string(Subject subject) => subject.Text;
}
