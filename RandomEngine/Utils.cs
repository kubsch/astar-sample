namespace RandomEngine;

public class Utils
{
    public Random Random { get; } = new();
    public Random RandomFix { get; } = new(1);
}