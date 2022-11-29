namespace AoC22;

public interface IDay
{
    public void Setup();
    public void SolvePart1();
    public void SolvePart2();
}

public abstract class Puzzle : IDay
{
    protected readonly ILogger _logger;
    protected readonly string _path;
    public Puzzle(ILogger logger, string path)
    {
        _logger = logger;
        _path = path;
    }

    public Puzzle(string path)
    {
        _logger = new ConsoleLogger();
        _path = path;
    }

    public virtual void Setup()
    {
    }

    public virtual void SolvePart1()
    {
        _logger.Log("Solving Part 1 Not Implemented Yet.");
    }

    public virtual void SolvePart2()
    {
        _logger.Log("Solving Part 2 Not Implemented Yet.");
    }
}