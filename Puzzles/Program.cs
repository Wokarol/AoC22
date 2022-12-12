using System;
using System.Collections.Generic;
using AoC22;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Perfolizer.Horology;

const int START_DAY = 1;
const int STOP_DAY = 25;

var logger = new ConsoleLogger();

for (int i = START_DAY; i <= STOP_DAY; i++)
{
    Puzzle puzzle;
    try
    {
        puzzle = Utils.GetClassOfType<Puzzle>($"Day{i.ToString("00")}", logger, Utils.FullPath(i));
        logger.Log($"\u001b[31mDay {i}:\u001b[0m");
    }
    catch (Exception) // e)
    {
        //logger.Log(e.Message);
        continue;
    }

    //Yikeusing (new Watch(""))
    {
        puzzle.Setup();

        puzzle.SolvePart1();
        puzzle.SolvePart2(); 
    }

    logger.Log("");
}

//BenchmarkRunner.Run<Bench>();

[Config(typeof(FastConfig))]
public class Bench
{
    public static IEnumerable<Puzzle> GetPuzzles()
    {
        var logger = new TestLogger();
        yield return new Day01(logger, Utils.FullPath(1));
        yield return new Day02(logger, Utils.FullPath(2));
        yield return new Day03(logger, Utils.FullPath(3));
        yield return new Day04(logger, Utils.FullPath(4));
        yield return new Day05(logger, Utils.FullPath(5));
        yield return new Day06(logger, Utils.FullPath(6));
        yield return new Day07(logger, Utils.FullPath(7));
        yield return new Day08(logger, Utils.FullPath(8));
        yield return new Day09(logger, Utils.FullPath(9));
        yield return new Day10(logger, Utils.FullPath(10));
        yield return new Day11(logger, Utils.FullPath(11));
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetPuzzles))]
    public void RunPuzzle(Puzzle puzzle)
    {
        puzzle.Setup();
        puzzle.SolvePart1();
        puzzle.SolvePart2();
    }
}
public class FastConfig : ManualConfig
{
    public FastConfig()
    {
        AddJob(Job.Default
            .WithWarmupCount(1)
            .WithLaunchCount(1));
    }
}