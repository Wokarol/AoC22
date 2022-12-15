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

    using (new Watch(""))
    {
        puzzle.Setup();

        puzzle.SolvePart1();
        puzzle.SolvePart2();
    }

    logger.Log("");
}

//BenchmarkRunner.Run<Bench>();

[MemoryDiagnoser]
public class Bench
{
    Puzzle puzzle1 = new Day13(new TestLogger(), Utils.FullPath(13));
    Puzzle puzzle2 = new Day13(new TestLogger(), Utils.FullPath(13));

    public void Setup()
    {
        puzzle2.Setup();
    }

    //[Benchmark]
    //public void Day13WithSetup()
    //{
    //    puzzle1.Setup();
    //    puzzle1.SolvePart1();
    //    puzzle1.SolvePart2();
    //}

    [Benchmark]
    public void Day13WithNoSetup()
    {
        puzzle2.SolvePart1();
        puzzle2.SolvePart2();
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