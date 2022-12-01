using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.Linq;

namespace AoC22;

public static class Utils
{
    #region Reflection

    public static T GetClassOfType<T>(string className, params object?[]? args)
    {
        var genericType = typeof(T).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)))
            .FirstOrDefault(t => t.Name == className);

        if (genericType is null)
            throw new Exception($"There is no class named {className}");

        if (Activator.CreateInstance(genericType, args) is not T instance)
            throw new Exception($"Somehow the class {className} does not implement {nameof(T)}... which should be impossible");

        return instance;
    }

    #endregion

    #region IO File Reading

    public static bool FileExists(string path) => File.Exists(path);

    public static string FullPath(int number, bool forTests = false, string fileName = "input.txt")
    {
        var folder = $"Day{number:D2}{(forTests ? "Test" : string.Empty)}";
        var file = fileName;
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder, file);
    }

    public static string[] ReadAllLines(string path) => FileExists(path) ? File.ReadAllLines(path) : Array.Empty<string>();

    public static IEnumerable<string> ReadFrom(string path)
    {
        if (!FileExists(path)) yield break;

        string? line;
        using var reader = File.OpenText(path);
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    #endregion

    #region Collection Extensions

    /// <summary>
    /// Adds the value or sets a key-value pair if one does not exist. Returns true if one was already in the dictionary
    /// </summary>
    public static bool AddToOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue val) where TValue : INumber<TValue>
    {
        if (dict.ContainsKey(key))
        {
            dict[key] += val;
            return true;
        }
        dict.Add(key, val);
        return false;
    }

    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue? defVal) => 
        dict.TryGetValue(key, out TValue? value) ? value : defVal;

    // works better on a collection with an Odd amount of elements
    public static T Median<T>(this IEnumerable<T> presortedCollection)
    {
        return presortedCollection.ElementAt(presortedCollection.Count() / 2);
    }

    public static void Swap<T>(this IList<T> list, int index1, int index2)
    {
        if (index1 == index2) return;
        (list[index2], list[index1]) = (list[index1], list[index2]);
    }

    public static void Swap<T>(this T[] array, int index1, int index2)
    {
        if (index1 == index2) return;
        (array[index2], array[index1]) = (array[index1], array[index2]);
    }

    /// <summary>
    /// Similar to Swap, but if the two indices aren't next to each other, everything in-between will shift over
    /// </summary>
    public static void SwapShift<T>(this IList<T> list, int from, int to)
    {
        if (from == to) return;
        T temp = list[from];
        list.RemoveAt(from);
        list.Insert(to, temp);
    }

    public static T MaxBy<T>(this IEnumerable<T> source, Func<T, IComparable> score) =>
        source.Aggregate((x, y) => score(x).CompareTo(score(y)) > 0 ? x : y);

    public static T MinBy<T>(this IEnumerable<T> source, Func<T, IComparable> score) =>
        source.Aggregate((x, y) => score(x).CompareTo(score(y)) < 0 ? x : y);

    #endregion

    #region Conversion Helpers

    public static int[] ConvertToInts(this string[] data)
    {
        return Array.ConvertAll(data, s => int.Parse(s));
    }

    public static int BinaryToInt(this string s) => Convert.ToInt32(s, 2);
    public static long BinaryToLong(this string s) => Convert.ToInt64(s, 2);
    public static int HexToInt(this string s) => Convert.ToInt32(s, 16);
    
    // Not sure if Convert.ToString(hexChar, 2) is a valid alternative. Should probably test...
    public static string HexToBinary(this char hexChar) => hexChar switch
    {
        '0' => "0000",
        '1' => "0001",
        '2' => "0010",
        '3' => "0011",
        '4' => "0100",
        '5' => "0101",
        '6' => "0110",
        '7' => "0111",
        '8' => "1000",
        '9' => "1001",
        'A' or 'a' => "1010",
        'B' or 'b' => "1011",
        'C' or 'c' => "1100",
        'D' or 'd' => "1101",
        'E' or 'e' => "1110",
        'F' or 'f' => "1111",
        _ => throw new NotImplementedException(),
    };

    #endregion

    #region Misc

    private static readonly Dictionary<int, int> _triangleLookup = new();
    /// <summary>
    /// Returns sum of 1 + 2 + ... + n-1 + n. Also known as Pascal's Triangle. 
    /// Like Factorial but for addition instead. Same result as n(n+1)/2.
    /// If you have a sequence like 1, 3, 6, 10, 15, 21, 28, ...
    /// </summary>
    public static int GetTriangleNumber(int n)
    {
        if (n < 0) return 0; // unhandled cases
        if (!_triangleLookup.TryGetValue(n, out int result))
        {
            result = (n * (n + 1)) >> 1;
            _triangleLookup.Add(n, result);
        }
        return result;
    }

    private static readonly Dictionary<int, int> _fibonacciLookup = new() { { 0, 0 }, { 1, 1 } };
    /// <summary>The famous Fibonacci sequence: 0, 1, 1, 2, 3, 5, 8, 13, 21, ...</summary>
    public static int GetFibonacci(int n)
    {
        if (n < 0) return 0; // avoids stackoverflow exception
        if (!_fibonacciLookup.TryGetValue(n, out int result))
        {
            result = GetFibonacci(n - 2) + GetFibonacci(n - 1);
            _fibonacciLookup.Add(n, result);
        }
        return result;
    }

    private static readonly Dictionary<int, int> _acceleratedSumLookup = new();
    // f(5) = 1 + 3 + 6 + 10 + 15 = 35 = n(n+1)(n+2)/6 = sequence 1, 4, 10, 20, 35, 56
    public static int GetAcceleratingSum(int index)
    {
        if (!_acceleratedSumLookup.TryGetValue(index, out int result))
        {
            result = index * (index + 1) * (index + 2) / 6;
            _acceleratedSumLookup.Add(index, result);
        }
        return result;
    }

    // TODO: Make a re-usable dictionary type designed for recursive lookups. Override the `this[]` accessor for safety

    private static readonly Dictionary<int, int> _miscSequence = new();
    /// <summary>
    /// For sequences that build from previous values. A Fibonacci sequence could go like:
    /// (dict, index) => index == 0 ? 0 : index == 1 ? 1 : dict[index - 1] + dict[index - 2];
    /// But that could throw exceptions if that index doesn't exist in the dictionary yet
    /// </summary>
    public static int MiscSequence(int index, Func<Dictionary<int, int>, int, int> rule)
    {
        if (!_miscSequence.TryGetValue(index, out int result))
        {
            result = rule(_miscSequence, index);
            _miscSequence.Add(index, result);
        }
        return result;
    }

    #endregion

    #region Vector2Int Extensions

    /// <summary>Horizontally or Vertically aligned, but not the same position</summary>
    public static bool IsLateralTo(this Vector2Int a, Vector2Int b) => a.X == b.X ^ a.Y == b.Y;

    public static void Reset(this ref Vector2Int v) => v = Vector2Int.Zero;

    public static IEnumerable<Vector2Int> GetRange(this Vector2Int from, Vector2Int to)
    {
        do
        {
            yield return from;
            from.X += from.X < to.X ? 1 : from.X > to.X ? -1 : 0;
            from.Y += from.Y < to.Y ? 1 : from.Y > to.Y ? -1 : 0;
        } while (from != to);
        yield return to;
    }

    #endregion

}

public sealed class Watch : IDisposable
{
    private readonly string _text;
    private readonly Stopwatch _watch;

    public Watch(string text)
    {
        _text = text;
        _watch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _watch.Stop();
        Console.WriteLine($"{_text}: {_watch.ElapsedMilliseconds}ms");
    }
}
