using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day14 : Puzzle
{
    private Map map;

    public Day14(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        map = new Map();

        foreach (var line in Utils.ReadAllLines(_path))
        {
            string[] segments = line.Split(" -> ");
            for (int i = 0; i < segments.Length - 1; i++)
            {
                var firstPair = segments[i].Split(',');
                var secondPair = segments[i + 1].Split(',');

                int x1 = int.Parse(firstPair[0]);
                int x2 = int.Parse(secondPair[0]);
                int y1 = int.Parse(firstPair[1]);
                int y2 = int.Parse(secondPair[1]);

                map.DrawLine(x1, y1, x2, y2);
            }
        }

        map.ExpandBoundBy(500, 0);
    }

    public override void SolvePart1()
    {
        HashSet<(int x, int y)> sand = new();
        Func<int, int, bool> isPointOccupied = (x, y) => sand.Contains((x, y)) || map.IsOccupied(x, y);

        int i = 0;
        while (true)
        {
            bool brokeOutOfMap = false;
            (int x, int y) pos = (500, 0);
            while (true)
            {
                if (!map.Bounds.Contains(pos.x, pos.y))
                {
                    brokeOutOfMap = true;
                    break;
                }

                if (!isPointOccupied(pos.x, pos.y + 1))
                {
                    pos = (pos.x, pos.y + 1);
                }
                else if (!isPointOccupied(pos.x - 1, pos.y + 1))
                {
                    pos = (pos.x - 1, pos.y + 1);
                }
                else if (!isPointOccupied(pos.x + 1, pos.y + 1))
                {
                    pos = (pos.x + 1, pos.y + 1);
                }
                else
                {
                    break;
                }
                //if (i > 110)
                //{
                //    map.PrintMap(_logger.Log, (500, 0), sand, depth: 40, newSand: pos);
                //    Console.ReadKey();
                //}
            }

            if (brokeOutOfMap)
            {
                break;
            }

            sand.Add(pos);
            i++;
        }
        _logger.Log(i);
    }

    public override void SolvePart2()
    {
        //map.PrintMap(_logger.Log, (500, 0));
     
        map.DrawLine(100, map.Bounds.YMax + 2, 900, map.Bounds.YMax + 2);

        HashSet<(int x, int y)> sand = new();
        Func<int, int, bool> isPointOccupied = (x, y) => sand.Contains((x, y)) || map.IsOccupied(x, y);

        int i = 0;
        while (true)
        {
            bool brokeOutOfMap = false;
            (int x, int y) pos = (500, 0);
            while (true)
            {

                if (!map.Bounds.Contains(pos.x, pos.y))
                {
                    _logger.Log("Floor too small, make it bigger");
                    brokeOutOfMap = true;
                    break;
                }

                if (!isPointOccupied(pos.x, pos.y + 1))
                {
                    pos = (pos.x, pos.y + 1);
                }
                else if (!isPointOccupied(pos.x - 1, pos.y + 1))
                {
                    pos = (pos.x - 1, pos.y + 1);
                }
                else if (!isPointOccupied(pos.x + 1, pos.y + 1))
                {
                    pos = (pos.x + 1, pos.y + 1);
                }
                else
                {
                    if (pos == (500, 0))
                    {
                        brokeOutOfMap = true;
                    }

                    break;
                }
                //if (i > 110)
                //{
                //    map.PrintMap(_logger.Log, (500, 0), sand, depth: 40, newSand: pos);
                //    Console.ReadKey();
                //}
            }

            if (brokeOutOfMap)
            {
                break;
            }

            sand.Add(pos);
            i++;
        }
        //map.PrintMap(_logger.Log, (500, 0), sand);
        _logger.Log(i + 1);
    }

    private class Map
    {
        private readonly HashSet<(int x, int y)> rocks = new();
        private bool wereBoundsInitialized = false;

        public (int x, int y) SandSpawner { get; set; }
        public Bounds Bounds { get; private set; }


        public bool IsOccupied(int x, int y)
        {
            return rocks.Contains((x, y));
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            int xmin = Math.Min(x1, x2);
            int xmax = Math.Max(x1, x2);
            int ymin = Math.Min(y1, y2);
            int ymax = Math.Max(y1, y2);

            if (wereBoundsInitialized)
            {
                var b = Bounds;
                b.Encapsulate(x1, y1);
                b.Encapsulate(x2, y2);
                Bounds = b;
            }
            else
            {
                Bounds = new Bounds(xmin, xmax, ymin, ymax);
                wereBoundsInitialized = true;
            }

            for (int x = xmin; x <= xmax; x++)
                for (int y = ymin; y <= ymax; y++)
                {
                    rocks.Add((x, y));
                }
        }

        public void PrintMap(Action<string> printLine, (int x, int y) sandSpawner, HashSet<(int x, int y)>? sand = null, int depth = -1, (int x, int y)? newSand = null)
        {
            Span<char> line = stackalloc char[Bounds.XMax - Bounds.XMin + 1];
            int yMax = depth != -1 ? depth : Bounds.YMax;
            for (int y = Bounds.YMin; y <= yMax; y++)
            {
                for (int x = Bounds.XMin; x <= Bounds.XMax; x++)
                {
                    line[x - Bounds.XMin] = rocks.Contains((x, y)) ? '#' : '.';

                    if (x == sandSpawner.x && y == sandSpawner.y)
                        line[x - Bounds.XMin] = '+';

                    if (sand != null && sand.Contains((x, y)))
                        line[x - Bounds.XMin] = 'o';

                    if (x == newSand?.x && y == newSand?.y)
                        line[x - Bounds.XMin] = '*';
                }
                printLine($"{line.ToString()}");
            }
        }

        public void ExpandBoundBy(int x, int y)
        {
            var b = Bounds;
            b.Encapsulate(x, y);
            Bounds = b;
        }
    }
}
