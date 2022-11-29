using System;

namespace AoC22;

public interface ILogger
{
    public void Log(string msg);
}

public class ConsoleLogger : ILogger
{
    public void Log(string msg)
    {
        Console.WriteLine(msg);
    }
}

public class TestLogger : ILogger
{
    public string? Message { get; private set; }

    public void Log(string msg)
    {
        Message = msg;
    }
}