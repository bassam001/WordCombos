using WordCombos.Core.Interfaces;

namespace WordCombos.Core.Strategies;

public sealed class TargetSelector : ITargetSelector
{
    public IReadOnlyList<string> SelectTargets(ISet<string> words, int targetLength)
    {
        var list = new List<string>(words.Count);
        foreach (var w in words) if (w.Length == targetLength) list.Add(w);
        return list;
    }
}
