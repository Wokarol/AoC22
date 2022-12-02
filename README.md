### Advent of Code 2022

Written in C# 11 (preview), .NET 7.0

Checkout the Template branch if you want to use it for a fresh project. Features:
- Uses Reflection to discover classes
- No need to update Program.cs unless you want to exclude certain Days from running
- Automated Tests: You just need to add a folder and 2 text files* for each day. (*with the right names)
	- Uses strings so no need to convert between int, long, string, or use dynamic
- Lots of Utility Helpers:
	- Node (+ A* Pathfinding)
	- Vector2Int
	- Bounds
	- Math extensions
	- IO file reading helpers