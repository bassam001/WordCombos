namespace WordCombos.Core.Models;

public readonly record struct Combination(string Target, IReadOnlyList<string> Parts);
