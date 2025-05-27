namespace CDL.TypeChecker;

[Flags]
public enum EventTypes
{
    Calendar = 0b1,
    Subject = 0b10,
    DateTime = 0b100,
    Description = 0b1000,
    Duration = 0b10000,
}
