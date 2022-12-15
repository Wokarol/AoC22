using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using static AoC22.Day12;

namespace AoC22;

public partial class Day15 : Puzzle
{
    HashSet<Vector2Int> taken = new();
    List<SensorReading> readings;

    int xStart = int.MaxValue;
    int xEnd = int.MinValue;

    public Day15(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        readings = new();
        var regex = ParseLineRegex();
        foreach (var line in Utils.ReadAllLines(_path))
        {
            var match = regex.Match(line);
            Vector2Int pos = new(
                int.Parse(match.Groups[1].ValueSpan),
                int.Parse(match.Groups[2].ValueSpan)
            );
            Vector2Int beacon = new(
                int.Parse(match.Groups[3].ValueSpan),
                int.Parse(match.Groups[4].ValueSpan)
            );
            int dist = ManhattanDistance(pos, beacon);

            taken.Add(pos);
            taken.Add(beacon);


            xStart = Math.Min(xStart, pos.X - dist);
            xEnd = Math.Max(xEnd, pos.X + dist);

            readings.Add(new(pos, beacon, dist));
        }
    }

    public override void SolvePart1()
    {
        if (true)
        {
            _logger.LogHighComputationTime();
            return;
        }

        int y = 2000000;

        Span<(int start, int end)> slices = stackalloc (int start, int end)[readings.Count];

        for (int i = 0; i < readings.Count; i++)
        {   
            int yDiff = y - readings[i].Pos.Y;
            int xOff = readings[i].Distance - Math.Abs(yDiff);

            slices[i] = (readings[i].Pos.X - xOff, readings[i].Pos.X + xOff);
        }

        int invalidSpots = 0;
        for (int x = xStart; x <= xEnd; x++)
        {
            if (taken.Contains(new(x, y)))
                continue;

            bool isWithinReachOfAnySensor = false;
            for (int i = 0; i < slices.Length; i++)
            {
                var (leftX, rightX) = slices[i];
                if (x >= leftX && x <= rightX)
                {
                    isWithinReachOfAnySensor = true;
                    break;
                }
            }

            if (isWithinReachOfAnySensor)
            {
                invalidSpots++;
            }
        }
        _logger.Log(invalidSpots);
    }

    public override void SolvePart2()
    {
        if (true)
        {
            _logger.LogHighComputationTime();
            return;
        }

        int max = 4000000;

        for (int i = 0; i < readings.Count; i++)
        {
            for (int j = i + 1; j < readings.Count; j++)
            {
                var iPos = readings[i].Pos;
                var jPos = readings[j].Pos;

                var dist = ManhattanDistance(iPos, jPos);

                if (dist == readings[i].Distance + readings[j].Distance + 2)
                {
                    foreach (var c in GetAllAround(readings[i].Pos, readings[i].Distance))
                    {
                        if (c.X < 0 || c.X > max) continue;
                        if (c.Y < 0 || c.Y > max) continue;

                        if (IsPointTarget(c.X, c.Y))
                        {
                            _logger.Log(c.X * 4000000L + c.Y);
                            return;
                        }
                    }
                }
            }
        }
    }

    private IEnumerable<Vector2Int> GetAllAround(Vector2Int pos, int dist)
    {
        int targDist = dist + 1;
        for (int i = 0; i <= targDist; i++)
        {
            int invI = targDist - i;
            yield return pos + new Vector2Int(i, invI);
            yield return pos + new Vector2Int(-i, invI);
            yield return pos + new Vector2Int(-i, -invI);
            yield return pos + new Vector2Int(i, -invI);
        }
    }

    private bool IsPointTarget(int x, int y)
    {
        Vector2Int cell = new(x, y);

        if (taken.Contains(cell))
            return false;

        for (int i = 0; i < readings.Count; i++)
        {
            Vector2Int sPos = readings[i].Pos;
            int dist = ManhattanDistance(sPos, cell);
            if (dist <= readings[i].Distance)
            {
                return false;
            }
        }

        return true;
    }

    private int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    [GeneratedRegex(@"Sensor\sat\sx=([\d-]+),\sy=([\d-]+):\sclosest\sbeacon\sis\sat\sx=([\d-]+),\sy=([\d-]+)")]
    private partial Regex ParseLineRegex();

    readonly struct SensorReading
    {
        public readonly Vector2Int Pos;
        public readonly Vector2Int ClosestBeacon;
        public readonly int Distance;

        public SensorReading(Vector2Int pos, Vector2Int closestBeacon, int distance)
        {
            Pos = pos;
            ClosestBeacon = closestBeacon;
            Distance = distance;
        }
    }
}
