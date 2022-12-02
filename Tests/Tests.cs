using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AoC22.Tests;

public class Tests
{
    const int START_DAY = 1;
    const int STOP_DAY = 25;

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
            Assert.That(part1Result, Is.EqualTo(expectedResult1), "Part 1 (no Part 2 provided)");
            Assert.Inconclusive("Part 1 Passed. Part 2 expected result not yet provided."); // optional, depending how you like to see test results
        }
        
        else if (expected.Length == 2 || expected.Length == 3 && string.IsNullOrWhiteSpace(expected[2]))
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
            Assert.Fail($"Expected 1-2 lines in results.txt, found {expected.Length}. Did not know how to parse.");
    }

    private static IEnumerable<TestCaseData> TestCaseGenerator()
    {
        Puzzle puzzle;
        string[] expected;
        for (int i = START_DAY; i <= STOP_DAY; i++)
        {
            try
            {
                if (!TestUtils.TryGetTestPath(i, out var inputPath)) continue;
                if (!TestUtils.TryGetTestPath(i, out var expectedPath, "results.txt")) continue;

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