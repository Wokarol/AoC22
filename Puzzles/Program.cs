using System;
using AoC22;

const int START_DAY = 1;
const int STOP_DAY = 25;

var logger = new ConsoleLogger();

for (int i = START_DAY; i <= STOP_DAY; i++)
{
	Puzzle puzzle;
	try
	{
		puzzle = Utils.GetClassOfType<Puzzle>($"Day{i}", logger, Utils.FullPath(i));
		logger.Log($"\u001b[31mDay {i}:\u001b[0m");
	}
	catch (Exception) // e)
	{
		//logger.Log(e.Message);
		continue;
	}

    puzzle.Setup();

    puzzle.SolvePart1();
    puzzle.SolvePart2(); 

	logger.Log("");
}