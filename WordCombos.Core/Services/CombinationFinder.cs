using WordCombos.Core.Interfaces;
using WordCombos.Core.Models;

namespace WordCombos.Core;

public sealed class CombinationFinder : ICombinationFinder
{
    private readonly ITargetSelector _targets;
    private readonly IMaxPartLengthProvider _maxLen;
    private readonly ISegmentationStrategy _strategy;

    public CombinationFinder(ITargetSelector targets, IMaxPartLengthProvider maxLen, ISegmentationStrategy strategy)
    {
        _targets = targets;
        _maxLen = maxLen;
        _strategy = strategy;
    }

    public IEnumerable<Combination> FindAll(ISet<string> words, int targetLength, int minParts = 2, int? maxParts = null)
    {
        if (words is null || words.Count == 0) yield break;
        if (targetLength < 1) yield break;
        if (minParts < 1) minParts = 1;

        var tgs = _targets.SelectTargets(words, targetLength);
        if (tgs.Count == 0) yield break;

        var maxPartLen = _maxLen.Get(words, targetLength);

        foreach (var t in tgs)
        {
            foreach (var parts in _strategy.Segment(t, words, maxPartLen, maxParts))
                if (parts.Count >= minParts)
                    yield return new Combination(t, parts);
        }
    }
}
