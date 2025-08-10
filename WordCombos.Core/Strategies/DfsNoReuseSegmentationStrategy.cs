using WordCombos.Core.Interfaces;

namespace WordCombos.Core;

public sealed class DfsNoReuseSegmentationStrategy : ISegmentationStrategy
{
    public IEnumerable<IReadOnlyList<string>> Segment(string target, ISet<string> dict, int maxPartLen, int? maxParts)
    {
        var path = new List<string>();
        var comparer = (dict as HashSet<string>)?.Comparer ?? StringComparer.Ordinal;
        var used = new HashSet<string>(comparer);

        foreach (var r in Dfs(0)) yield return r;

        IEnumerable<IReadOnlyList<string>> Dfs(int i)
        {
            if (i == target.Length)
            {
                yield return path.ToArray();
                yield break;
            }

            int remaining = target.Length - i;
            int tryMax = maxPartLen < remaining ? maxPartLen : remaining;

            for (int len = 1; len <= tryMax; len++)
            {
                var slice = target.AsSpan(i, len).ToString();
                if (!dict.Contains(slice)) continue;
                if (used.Contains(slice)) continue;
                if (maxParts.HasValue && path.Count + 1 > maxParts.Value) continue;

                path.Add(slice);
                used.Add(slice);
                foreach (var r in Dfs(i + len)) yield return r;
                used.Remove(slice);
                path.RemoveAt(path.Count - 1);
            }
        }
    }
}
