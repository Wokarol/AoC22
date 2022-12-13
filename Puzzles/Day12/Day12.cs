using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day12 : Puzzle
{
    int[,] grid;
    (int x, int y) start;
    List<(int x, int y)> lowPoints = new();
    (int x, int y) end;

    public Day12(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        var lines = Utils.ReadAllLines(_path);

        grid = new int[lines[0].Length, lines.Length];
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                char c = lines[y][x];

                grid[x, y] = c switch
                {
                    'S' => 'a' - 'a',
                    'E' => 'z' - 'a',
                    _ => c - 'a',
                };

                if (c == 'S') start = (x, y);
                if (c == 'E') end = (x, y);
                if (c == 'a') lowPoints.Add((x, y));
            }
    }

    public override void SolvePart1()
    {
        if (true)
        {
            _logger.Log("\u001b[30;1mSolution disabled due to high computation time\u001b[0m");
            return;
        }

        var nodes = CreateNodeList();
        var astar = new Astar(nodes);
        var path = astar.FindPath(start, end);

        _logger.Log(path.Count);
    }


    public override void SolvePart2()
    {
        if (true)
        {
            _logger.Log("\u001b[30;1mSolution disabled due to high computation time\u001b[0m");
            return;
        }

        Stack<Node>? cachedPathStack = new();
        List<(int heightDiff, Node node)>? cachedAdjacencies = new();
        List<Node>? cachedList = new();
        PriorityQueue<Node, float>? cachedPriorityQueue = new();

        int min = int.MaxValue;
        var nodes = CreateNodeList();
        var astar = new Astar(nodes);
        foreach (var p in lowPoints)
        {
            bool isNextTo1 = false;
            if (p.x >= 1 && grid[p.x - 1, p.y] == 1) isNextTo1 = true;
            if (p.y >= 1 && grid[p.x, p.y - 1] == 1) isNextTo1 = true;
            if (p.x <= grid.GetLength(0) - 2 && grid[p.x + 1, p.y] == 1) isNextTo1 = true;
            if (p.y <= grid.GetLength(1) - 2 && grid[p.x, p.y + 1] == 1) isNextTo1 = true;

            if (!isNextTo1) continue;
            var path = astar.FindPath(p, end, cachedPathStack, cachedPriorityQueue, cachedList, cachedAdjacencies);

            if (path == null)
                continue;

            if (path.Count < min)
            {
                min = path.Count;
            }
        }
        _logger.Log(min);
    }

    private List<List<Node>> CreateNodeList()
    {
        List<List<Node>> nodes = new List<List<Node>>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            nodes.Add(new());
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                nodes[^1].Add(new Node((x, y), grid[x, y]));
            }
        }

        return nodes;
    }

    private void ComputeAndDrawMap(Stack<Node>? path)
    {
        char[,] map = new char[grid.GetLength(0), grid.GetLength(1)];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                map[x, y] = '.';

                if ((x, y) == end)
                    map[x, y] = 'E';
            }
        }

        foreach (var n in path)
        {
            if ((n.Position.X, n.Position.Y) != end)
                map[n.Position.X, n.Position.Y] = 'x';

            var parent = n.Parent.Position;

            var diff = (n.Position.X - parent.X, n.Position.Y - parent.Y);
            map[n.Position.X - diff.Item1, n.Position.Y - diff.Item2] = diff switch
            {
                (1, 0) => '>',
                (-1, 0) => '<',
                (0, 1) => 'v',
                (0, -1) => '^',
                _ => 'x',
            };
        }

        for (int y = 0; y < map.GetLength(1); y++)
        {
            string s = "";
            for (int x = 0; x < map.GetLength(0); x++)
            {
                s += map[x, y];
            }

            _logger.Log(s);
        }
    }

    // Borowed and modified from https://github.com/davecusatis/A-Star-Sharp/blob/master/Astar.cs
    public class Node
    {
        public Node? Parent;
        public (int X, int Y) Position;

        public float DistanceToTarget;
        public float Cost;
        public float Weight;

        // My params
        public int Height;

        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }

        public Node((int x, int y) pos, int height, float weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            Height = height;
        }
    }

    public class Astar
    {
        List<List<Node>> Grid;

        int GridRows
        {
            get
            {
                return Grid[0].Count;
            }
        }
        int GridCols
        {
            get
            {
                return Grid.Count;
            }
        }

        public Astar(List<List<Node>> grid)
        {
            Grid = grid;
        }

        public Stack<Node>? FindPath((int x, int y) Start, (int x, int y) End, Stack<Node>? cachedPathStack = null, PriorityQueue<Node, float>? cachedPriorityQueue = null, List<Node>? cachedList = null, List<(int heightDiff, Node node)>? cachedAdjacencies = null)
        {
            Node start = Grid[Start.x][Start.y];
            Node end = Grid[End.x][End.y];

            Stack<Node> Path = cachedPathStack ?? new();
            PriorityQueue<Node, float> OpenList = cachedPriorityQueue ?? new();
            List<Node> ClosedList = cachedList ?? new();
            List<(int heightDiff, Node node)> adjacencies = cachedAdjacencies ?? new();

            cachedPathStack?.Clear();
            cachedPriorityQueue?.Clear();
            cachedList?.Clear();
            cachedAdjacencies?.Clear();

            Node current = start;

            // add start node to Open List
            OpenList.Enqueue(start, start.F);

            while (OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);

                GetAdjacentNodes(current, adjacencies);

                foreach (var (heightDiff, n) in adjacencies)
                {
                    if (!ClosedList.Contains(n))
                    {
                        bool isFound = false;
                        foreach (var oLNode in OpenList.UnorderedItems)
                        {
                            if (oLNode.Element == n)
                            {
                                isFound = true;
                            }
                        }
                        if (!isFound)
                        {
                            n.Parent = current;
                            n.DistanceToTarget = 1;
                            n.Cost = n.Weight +/* heightDiff + */n.Parent.Cost;
                            OpenList.Enqueue(n, n.F);
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Node? temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null);
            return Path;
        }

        private void GetAdjacentNodes(Node n, List<(int heightDiff, Node node)> result)
        {
            // Alter to check height
            result.Clear();

            int row = n.Position.Y;
            int col = n.Position.X;

            if (row + 1 < GridRows)
            {
                AddTemp(result, col, row + 1, col, row);
            }
            if (row - 1 >= 0)
            {
                AddTemp(result, col, row - 1, col, row);
            }
            if (col - 1 >= 0)
            {
                AddTemp(result, col - 1, row, col, row);
            }
            if (col + 1 < GridCols)
            {
                AddTemp(result, col + 1, row, col, row);
            }



            void AddTemp(List<(int heightDiff, Node node)> temp, int col, int row, int myCol, int myRow)
            {
                var candidate = Grid[col][row];
                var heightDiff = candidate.Height - Grid[myCol][myRow].Height;

                if (heightDiff <= 1)
                {
                    temp.Add((heightDiff, candidate));
                }
            }
        }
    }
}
