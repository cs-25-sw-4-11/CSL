namespace CSL.TypeChecker;

[Flags]
public enum EventTypes
{
    Calendar = 0,
    Subject = 0b1,
    DateTime = 0b10,
    Description = 0b100,
    Duration = 0b1000,
}
