using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC22;

public class Node
{
    public Node? Connection { get; private set; }
    public List<Node>? Neighbors { get; private set; }

    /// <summary>Coordinate of this Node</summary>
    public Vector2Int Pos;

    /// <summary>Cost to go to this node</summary>
    public int Value { get; private set; }
    /// <summary>Cost from Start (all previous Values + this Value)</summary>
    public int G { get; private set; }
    /// <summary>Distance to target node. Aids in traveling more directly</summary>
    public int H { get; private set; }
    /// <summary>Total Heuristic for traveling to this node</summary>
    public int F => G + H;

    public Node(int x, int y, int value)
    {
        Pos = new(x, y);
        Value = value;
    }

    public void SetG(int val) => G = val;
    public void SetH(int val) => H = val;
    public int GetDistance(Node target) => Vector2Int.Distance(target.Pos, Pos);
    public void SetConnection(Node node) => Connection = node;
    public void AssignNeighbors(IEnumerable<Node> allNodes)
    {
        Neighbors = new();
        Neighbors.AddRange(allNodes.Where(n => Vector2Int.AreAdjacent(n.Pos, Pos)));
    }

    #region Pathfinding

    public static List<Node> FindPath<T>(Node start, Node end)
    {
        var toSearch = new List<Node>() { start };
        var processed = new List<Node>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var next in toSearch)
            {
                if (next.F < current.F || (next.F == current.F && next.H < current.H))
                {
                    current = next;
                }
            }

            processed.Add(current);
            toSearch.Remove(current);

            if (current == end)
            {
                var path = new List<Node>();
                var currentNode = end;
                while (currentNode != start)
                {
                    path.Add(currentNode!);
                    currentNode = currentNode!.Connection;
                }
                path.Add(start);
                return path;
            }

            foreach (var neighbor in current!.Neighbors!.Where(n => !processed.Contains(n)))
            {
                var inSearch = toSearch.Contains(neighbor);
                var costToNeighbor = current.G + neighbor.Value;

                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbor.SetH(neighbor.GetDistance(end));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
        return new List<Node>();
    }
    
    #endregion
}
