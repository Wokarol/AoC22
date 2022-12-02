## Advent of Code 2022

*Written in C# 11 (preview), .NET 7.0*

Checkout the <a href="https://github.com/Libberator/AoC22/tree/Template">Template branch</a> if you want to use this for a fresh project.

### Template Features
- Uses Reflection to discover classes
- No need to update Program.cs unless you want to exclude certain Days from running
- Automated Tests: You just need to add a folder and 2 text files* for each day (*with the right names)
	- Uses strings so no need to convert between int, long, string, or use dynamic
- Lots of Utility Helpers:
	- Collection extensions
	- Math extensions (primes, factors, misc formulas)
	- IO file reading helpers
- Custom classes/structs:
	- Node (w/ A* Pathfinding)
	- Vector2Int + Extension methods
	- Bounds
	- Grid (work in progress)

### Solutions
In following with the "make it work, make it right, make it fast" mantra, I try to write my solutions as cleanly as possible.
According to a FAQ for AoC:
> every problem has a solution that completes in at most 15 seconds on ten-year-old hardware.

Optimizing for performance can lead to illegible code, and I value maintainability and readability over *"the most optimal solution"*. 
For example, I'll use Linq because it can be more succint and is often easier to parse. Anyways, enjoy!

:writing_hand: Always open to feedback. Feel free to send a PR or open a ticket! :computer: 

:star: Leave a Star if you got any value from this :star: