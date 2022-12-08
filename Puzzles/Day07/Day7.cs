using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Kevin
{
    [GeneratedRegex("\\$ cd ([/.\\w]+)")]
    private static partial Regex ChangeDirectoryPattern();
}

public class Day7 : Puzzle
{
    private Directory root = new();

    public Day7(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        Stack<Directory> position = new();
        position.Push(root);

        bool shouldBeListing = false;
        foreach (var line in Utils.ReadFrom(_path))
        {
            if (line.StartsWith('$'))
            {
                shouldBeListing = false;
                if (line == "$ ls")
                {
                    // List directory
                    shouldBeListing = true;
                }
                else
                {
                    var match = Regex.Match(line, @"\$ cd (.+)");
                    string target = match.Groups[1].Value;
                    if (target == "/")
                    {
                        // Back to root dir
                        position.Clear();
                        position.Push(root);
                    }
                    else if (target == "..")
                    {
                        // Directory up
                        position.Pop();
                    }
                    else
                    {
                        // Directory down
                        var curr = position.Peek();
                        position.Push(curr.GetOrCreateDirectory(target));
                    }
                }
            }
            else
            {
                if (!shouldBeListing)
                    throw new InvalidOperationException("I should not list now");

                if (line.StartsWith("dir"))
                {
                    // Line is dir
                }
                else
                {
                    // Line is file
                    var match = Regex.Match(line, @"(\d+) (.+)");
                    var size = int.Parse(match.Groups[1].ValueSpan);
                    var name = match.Groups[2].Value;

                    position.Peek().Files.Add(name, size);
                }
            }
        }
    }

    public override void SolvePart1()
    {
        var sum = GetAllDirectorySizes(root)
            .Where(x => x <= 100_000)
            .Sum();
        _logger.Log(sum);
    }


    public override void SolvePart2()
    {
        int total = 70000000;
        int needed = 30000000;
        int used = GetAllDirectorySizes(root).Last();
        int freespace = total - used;
        int target = needed - freespace;

        _logger.Log(GetAllDirectorySizes(root)
            .OrderBy(x => x)
            .First(x => x >= target));
    }

    private void PrintTree(Directory d, int indent = 0)
    {
        if (indent == 0) _logger.Log($"/ [{GetAllDirectorySizes(d).Last()}]");

        foreach (var dir in d.Directories)
        {
            _logger.Log($"{"".PadLeft((indent + 1) * 2)}{dir.Key} [{GetAllDirectorySizes(dir.Value).Last()}]");
            PrintTree(dir.Value, indent + 1);
        }

        foreach (var f in d.Files)
        {
            _logger.Log($"{"".PadLeft((indent + 1) * 2)}> {f.Key} [{f.Value}]");
        }
    }

    private IEnumerable<int> GetAllDirectorySizes(Directory d)
    {
        int size = 0;

        foreach (var file in d.Files)
        {
            size += file.Value;
        }

        foreach (var directory in d.Directories)
        {
            int last = 0;
            foreach (var dir in GetAllDirectorySizes(directory.Value))
            {
                last = dir;
                yield return dir;
            }
            size += last;
        }

        yield return size;
    }

    class Directory
    {
        public readonly Dictionary<string, Directory> Directories = new();
        public readonly Dictionary<string, int> Files = new();

        public Directory GetOrCreateDirectory(string name)
        {
            if (!Directories.TryGetValue(name, out var dir))
            {
                dir = new();
                Directories[name] = dir;
            }
            return dir;
        }
    }
}
