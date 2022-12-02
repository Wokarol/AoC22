using System.Collections.Generic;
using System.Numerics;

namespace AoC22;

// work in progress. needs more functionality

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
}