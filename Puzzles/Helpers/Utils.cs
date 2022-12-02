﻿using System;
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

    public static string FullPath(int number, string file = "input.txt")
    {
        var folder = $"Day{number:D2}";
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

    #region String Conversions

    public static int[] ConvertToInts(this string[] data) => Array.ConvertAll(data, int.Parse);
    public static int BinaryToInt(this string s) => Convert.ToInt32(s, 2);
    public static long BinaryToLong(this string s) => Convert.ToInt64(s, 2);
    public static int HexToInt(this string s) => Convert.ToInt32(s, 16);
    
    // Can also consider Convert.ToString(hexChar, 2), but won't guarantee length of 4
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
    /// For sequences like 1, 3, 6, 10, 15, 21, 28, ...
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

    // Note: To find mathematical formulas for specific sequences, go to https://oeis.org/

    // TODO: Make a class for recursion, containing dictionary and methods that takes an index, Func and/or Predicate as args.

    #endregion

    #region Math Helpers

    /// <summary>
    /// Returns a sorted list of all the factors of <paramref name="n"/>. 
    /// Throws exception if <paramref name="n"/> is negative or 0.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static List<int> Factor(int n)
    {
        if (n < 1) throw new ArgumentOutOfRangeException($"n must be greater than 0. Value given: {n}");
        List<int> factors = new() { 1 };
        var upperLimit = (int)Math.Sqrt(n); // casting to int automatically floors
        for (var i = upperLimit; i >= 2; i--)
        {
            if (n % i == 0)
            {
                factors.Insert(1, i);
                var pair = n / i;
                if (i != pair) factors.Add(pair);
            }
        }
        if (n > 1) factors.Add(n);
        return factors;
    }

    // Note: This "IsPrime" method is a "naive" implementation.
    // For values greater than 2^14, see Miller-Rabin for a quicker approach: https://cade.site/diy-fast-isprime
    /// <summary>Checks if <paramref name="n"/> is prime: greater than 1 with no positive divisors other than 1 and itself.</summary>
    public static bool IsPrime(this int n) => IsPrime((long)n);
    /// <summary>Checks if <paramref name="n"/> is prime: greater than 1 with no positive divisors other than 1 and itself.</summary>
    public static bool IsPrime(this long n)
    {
        if (n <= 1) return false;
        return n.FirstPrimeFactor() == n;
    }

    /// <summary>
    /// This will return the first prime number that <paramref name="n"/> is divisible by.
    /// If <paramref name="n"/> is 1 or prime, it will return itself. Negative values throw exception.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static int FirstPrimeFactor(this int n) => (int)FirstPrimeFactor((long)n);
    /// <summary>
    /// This will return the first prime number that <paramref name="n"/> is divisible by.
    /// If <paramref name="n"/> is 1 or prime, it will return itself. Negative values throw exception.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static long FirstPrimeFactor(this long n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException($"n must be a positive integer. Value given: {n}");
        if ((n & 1) == 0) return 2;

        for (int d = 3; d * d <= n; d += 2)
            if (n % d == 0) return d;

        return n;
    }

    /// <summary>Returns the greatest common divisor of the two arguments.</summary>
    public static int GreatestCommonDivisor(int a, int b) => b > 0 ? GreatestCommonDivisor(b, a % b) : Math.Abs(a);

    /// <summary>Returns the least common multiple of the two arguments.</summary>
    public static int LeastCommonMultiple(int a, int b) => (a * b) / GreatestCommonDivisor(a, b);

    #endregion

}

/// <summary>For referencing a value type as a reference type. Useful if you want to edit a value inside of a Stack/Queue.</summary>
public class Wrapper<T> where T : struct
{ 
    public T Value { get; set; } 
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
