using WordCombos.Core.Interfaces;

namespace WordCombos.Core.Strategies;

public sealed class MaxPartLengthProvider : IMaxPartLengthProvider
{
    public int Get(ISet<string> words, int targetLength)
    {
        var max = 0;
        foreach (var w in words) if (w.Length > max) max = w.Length;
        if (max > targetLength) max = targetLength;
        return max;
    }
}
