using System.Collections.Generic;
using System.Numerics;

namespace AoC22;

// Work in Progress
public class Grid<T>
{
    // Not sure if this should be a Dictionary, List, multidimensional array [,], jagged array [][],...
    public readonly Dictionary<Vector2Int, T> Data = new();

    private readonly int _width; // columns. x-value
    private readonly int _height; // rows. y-value

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public bool IsWithinBounds(Vector2Int pos) => IsWithinBounds(pos.X, pos.Y);
    public bool IsWithinBounds(int x, int y) => x >= 0 && x < _width && y >= 0 && y < _height;

    /*
        TODO Features:
        Slice (for copy or cut), top-left pos, width, height
        Paste (and similarly, Fill)
        FlipX/FlipY
        Rotate
        Shift
        Get, Set
        *, +, -
        Foreach
    */

    /*
        Grid - to - graph
        Add a pretty drawer for multi-dimensional array
        characters for empty space / wall / node / etc
        A---C---F
        |   |   |
        |   D---G
        |   |
        B---E 
    */
}