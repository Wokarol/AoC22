using System.Numerics;

namespace Puzzles;

public class Puzzle
{

    public static T GetGenericZero<T>() where T : INumber<T>
    {
        return T.Zero;
    }
}