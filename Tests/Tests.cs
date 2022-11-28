using Puzzles;

namespace AoCTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var zero = Puzzle.GetGenericZero<float>();

        Assert.That(zero, Is.EqualTo(0f));
    }
}