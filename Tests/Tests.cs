using AoC22;

namespace AoCTests;

public class Tests
{
    private static readonly ILogger _logger = new TestLogger();

    [Test]
    [TestCaseSource(nameof(TestCaseGenerator))]
    public void TestPuzzle(Puzzle puzzle, string[] expected)
    {
        // Arrange
        puzzle.Setup();
        
        if (expected.Length == 0)
            Assert.Fail("results.txt has no values in it to compare against.");

        else if (expected.Length == 1)
        {
            // Arrange
            var expectedResult1 = expected[0];
            
            // Act
            puzzle.SolvePart1();
            var part1Result = _logger.LastMessage;
            
            // Assert
            Assert.That(part1Result, Is.EqualTo(expectedResult1), "Part 1");
            Assert.Inconclusive("Part 1 Passed. Part 2 expected result not yet provided.");
        }

        else if (expected.Length == 2)
        {
            // Arrange
            var expectedResult1 = expected[0];
            var expectedResult2 = expected[1];
            
            // Act
            puzzle.SolvePart1();
            var part1Result = _logger.LastMessage;
            puzzle.SolvePart2();
            var part2Result = _logger.LastMessage;
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(part1Result, Is.EqualTo(expectedResult1), "Part 1");
                Assert.That(part2Result, Is.EqualTo(expectedResult2), "Part 2");
            });
        }

        else
            Assert.Inconclusive($"Expected 1-2 lines in results.txt, found {expected.Length}. Did not know how to parse.");
    }

    private static IEnumerable<TestCaseData> TestCaseGenerator()
    {
        Puzzle puzzle;
        string[] expected;
        for (int i = 1; i <= 25; i++)
        {
            try
            {
                var inputPath = Utils.FullPath(i, forTests: true);
                if (!Utils.FileExists(inputPath))
                    continue;

                var expectedPath = Utils.FullPath(i, forTests: true, fileName: "results.txt");
                if (!Utils.FileExists(expectedPath))
                    continue;

                puzzle = Utils.GetClassOfType<Puzzle>($"Day{i}", _logger, inputPath);
                expected = Utils.ReadAllLines(expectedPath);
            }
            catch (Exception)
            {
                continue;
            }
            yield return new TestCaseData(puzzle, expected).SetName($"Day {i}");
        }
    }
}