using System;

namespace AoC22;

public interface ILogger
{
    public string? LastMessage { get; }
    public void Log(string msg);
}

public class ConsoleLogger : ILogger
{
    public string? LastMessage { get; private set; }

    public void Log(string msg)
    {
        LastMessage = msg;
        Console.WriteLine(msg);
    }
}

public class TestLogger : ILogger
{
    public string? LastMessage { get; private set; }

    public void Log(string msg) => LastMessage = msg;
}

// TODO: maybe add a Write to Local Disk Logger?