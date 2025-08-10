This is my solution for the Peripass Technical Developer Test.

WordCombos — 
Finds all combinations of words (from an input list) that concatenate to X -letter target which itself also exists in the input.
________________________________________
Problem 
Given input.txt (output all combinations of words that:
1.	Concatenate to a target of length  and
2.	That target also exists in input.txt.
Example:
Input:
foobar
fo
o
bar
target of length 6 
Output:
fo+o+bar=foobar
________________________________________
What I built
•	A clean, testable core library with small interfaces and pluggable strategies.
•	Multiple segmentation strategies (DFS/DP; with & without re-use) to compare correctness & performance.
•	A simple WPF app that reads input.txt and prints valid combinations.
•	xUnit + Moq tests for the main behaviors.
________________________________________
	ITargetSelector – picks targets of the requested length.
	IMaxPartLengthProvider – decides max piece length to try.
	ISegmentationStrategy – the algorithm that splits a target to 
	Strategies implemented:
	DfsReuseSegmentationStrategy  DFS + memoization, allows reuse of the same word value multiple times.
	DfsNoReuseSegmentationStrategy DFS + backtracking, no reuse (uses a HashSet<string> to block duplicates by value
	DpReuseSegmentationStrategy  bottom-up DP that allows reuse
	DpNoReuseSegmentationStrategy – bottom-up DP that prevents reuse 
Why both DFS and DP?
	DFS yields lazily (memory-friendly for large result sets), easy to prune
	DP is fast when many overlapping subproblems exist but stores more in memory
________________________________________
How it runs
Requirements
•	.NET 8 SDK or 9
Structure
WordCombos.sln
/WordCombos.Adapters
  /WordCombos.Core      ← core logic & strategies
  /WordCombos.WPFApp       ← WPF app
  /WordCombos.Tests     ← xUnit + Moq tests
input.txt               test data
Build & run 
dotnet restore
dotnet build 
dotnet run --project WordCombos.App -- \
