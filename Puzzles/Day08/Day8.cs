using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public class Day8 : Puzzle
{

    int[,] grid;
    int[,] mask;

    public Day8(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        var lines = Utils.ReadAllLines(_path);
        grid = new int[lines[0].Length, lines.Length];
        mask = new int[lines[0].Length, lines.Length];

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[x, y] = lines[y][x] - '0';
                mask[x, y] = 0;
            }
        }

    }

    public override void SolvePart1()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            WalkGrid(i, 0, 0, 1);
            WalkGrid(i, grid.GetLength(0) - 1, 0, -1);
        }

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            WalkGrid(0, i, 1, 0);
            WalkGrid(grid.GetLength(1) - 1, i, -1, 0);
        }

        int sum = 0;
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                sum += mask[x, y] > 0 ? 1 : 0;
            }
        }

        _logger.Log(sum);
    }

    public override void SolvePart2()
    {
        int max = 0;
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                int right = WalkAndCount(x, y, 1, 0);
                int left = WalkAndCount(x, y, -1, 0);
                int down = WalkAndCount(x, y, 0, 1);
                int up = WalkAndCount(x, y, 0, -1);
                var count = right * left * down * up;

                if (count > max)
                    max = count;
            }
        }
        _logger.Log(max);
    }

    private void WalkGrid(int x, int y, int dx, int dy)
    {
        int highestTree = -1;
        while (true)
        {
            int currentTree = grid[x, y];

            if (currentTree > highestTree)
            {
                mask[x, y] = 1;
            }

            x += dx;
            y += dy;

            if (x < 0 || x >= grid.GetLength(0)) return;
            if (y < 0 || y >= grid.GetLength(1)) return;


            if (currentTree > highestTree)
                highestTree = currentTree;
        }
    }

    private int WalkAndCount(int x, int y, int dx, int dy)
    {
        int originalTree = grid[x, y];
        int sum = 0;

        x += dx;
        y += dy;

        if (x < 0 || x >= grid.GetLength(0)) return sum;
        if (y < 0 || y >= grid.GetLength(1)) return sum;

        while (true)
        {
            int currentTree = grid[x, y];

            if (currentTree < originalTree)
            {
                sum += 1;
            }
            if (currentTree >= originalTree)
            {
                return sum + 1;
            }

            x += dx;
            y += dy;

            if (x < 0 || x >= grid.GetLength(0)) return sum;
            if (y < 0 || y >= grid.GetLength(1)) return sum;
        }
    }

    private void PrintGrid(int[,] grid)
    {
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            string s = "";
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                s += grid[x, y].ToString();
            }
            _logger.Log(s);
        }
    }
}
