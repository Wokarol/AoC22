using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day15 : Puzzle
{
    List<SensorReading> readings;

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

            readings.Add(new(pos, beacon, dist));
        }
    }

    public override void SolvePart1()
    {
        int y = 2000000;
        List<SensorReading> relevantSensors = new();
        HashSet<int> taken = new();
        int xStart = int.MaxValue;
        int xEnd = int.MinValue;

        for (int i = 0; i < readings.Count; i++)
        {
            var reading = readings[i];
            if (Math.Abs(y - reading.Pos.Y) <= reading.Distance)
            {
                relevantSensors.Add(reading);
                xStart = Math.Min(xStart, reading.Pos.X - reading.Distance);
                xEnd = Math.Max(xEnd, reading.Pos.X + reading.Distance);
            }

            if (reading.ClosestBeacon.Y == y) taken.Add(reading.ClosestBeacon.X);
            if (reading.Pos.Y == y) taken.Add(reading.Pos.X);
        }

        int invalidSpots = 0;
        for (int x = xStart; x <= xEnd; x++)
        {
            if (taken.Contains(x))
                continue;

            Vector2Int cell = new(x, y);
            bool isWithinReachOfAnySensor = false;
            for (int i = 0; i < relevantSensors.Count; i++)
            {
                Vector2Int sPos = relevantSensors[i].Pos;
                int dist = ManhattanDistance(sPos, cell);
                if (dist <= relevantSensors[i].Distance)
                {
                    isWithinReachOfAnySensor = true;
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
        List<SensorReading> relevantSensors = new();
        HashSet<int> taken = new();
        int xStart = 0;
        int xEnd = 4000000;
        int yStart = 0;
        int yEnd = 4000000;

        for (int y = yStart; y < yEnd; y++)
        {
            var x = SearchLine(y, relevantSensors, taken, xStart, xEnd);
            if (x != null)
            {
                _logger.Log($"Found it at x = {x.Value} y = {y}");
                _logger.Log(x.Value * 4000000 + y);
            }

            if (y % 4000 == 0)
                _logger.Log($"Checked up to {(y * 100) / (float)yEnd:f2}%");
        }
    }

    private int? SearchLine(int y, List<SensorReading> relevantSensors, HashSet<int> taken, int xStart, int xEnd)
    {
        relevantSensors.Clear();
        taken.Clear();

        for (int i = 0; i < readings.Count; i++)
        {
            var reading = readings[i];
            if (Math.Abs(y - reading.Pos.Y) <= reading.Distance)
                relevantSensors.Add(reading);

            if (reading.ClosestBeacon.Y == y) taken.Add(reading.ClosestBeacon.X);
            if (reading.Pos.Y == y) taken.Add(reading.Pos.X);
        }

        for (int x = xStart; x <= xEnd; x++)
        {
            if (taken.Contains(x))
                continue;

            Vector2Int cell = new(x, y);
            bool isWithinReachOfAnySensor = false;
            for (int i = 0; i < relevantSensors.Count; i++)
            {
                Vector2Int sPos = relevantSensors[i].Pos;
                int dist = ManhattanDistance(sPos, cell);
                if (dist <= relevantSensors[i].Distance)
                {
                    isWithinReachOfAnySensor = true;
                }
            }

            if (!isWithinReachOfAnySensor)
            {
                return x;
            }
        }
        return null;
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
