using System;
using System.IO;

namespace AoC22.Tests;

public static class TestUtils
{
    public static bool TryGetTestPath(int number, out string fullPath, string file = "input.txt")
    {
        var folder = $"Day{number:D2}Test";
        fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder, file);
        return File.Exists(fullPath);
    }
}