using AoC22;

namespace AoCTests;

public class Tests
{
    private static readonly TestLogger _logger = new();

    [Test]
    [TestCaseSource(nameof(Generator))]
    public void Test(Puzzle puzzle, int day)
    {
        // Arrange
        puzzle.Setup();
        var path = Utils.FullPath(day, forTests: true, fileName: "results.txt");
        if (!Utils.FileExists(path))
            Assert.Fail("Missing results.txt for Test expectations");
        var expectations = Utils.ReadAllLines(path);
        var expectedResult1 = expectations[0]; // watch out for empty files; Index out of Range
        var expectedResult2 = expectations[^1];

        // Act
        puzzle.SolvePart1();
        var part1Result = _logger.Message;
        puzzle.SolvePart2();
        var part2Result = _logger.Message;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(part1Result, Is.EqualTo(expectedResult1), "Part 1");
            Assert.That(part2Result, Is.EqualTo(expectedResult2), "Part 2");
        });
    }

    private static IEnumerable<TestCaseData> Generator()
    {
        Puzzle puzzle;
        for (int i = 1; i <= 25; i++)
        {
            try
            {
                var path = Utils.FullPath(i, forTests: true);
                puzzle = Utils.GetClassOfType<Puzzle>($"Day{i}", _logger, path);
            }
            catch (Exception)
            {
                continue;
            }
            yield return new TestCaseData(puzzle, i).SetName($"Day {i}");
        }
    }
}