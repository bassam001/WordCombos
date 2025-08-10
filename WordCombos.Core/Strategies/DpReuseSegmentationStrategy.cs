using WordCombos.Core.Interfaces;

namespace WordCombos.Core;

public sealed class DpReuseSegmentationStrategy : ISegmentationStrategy
{
    public IEnumerable<IReadOnlyList<string>> Segment(string target, ISet<string> dict, int maxPartLen, int? maxParts)
    {
        var dp = new List<List<string>>[target.Length + 1];
        dp[0] = new List<List<string>> { new List<string>() };

        for (int i = 0; i < target.Length; i++)
        {
            var combosAtI = dp[i];
            if (combosAtI == null) continue;

            int remaining = target.Length - i;
            int tryMax = maxPartLen < remaining ? maxPartLen : remaining;

            for (int len = 1; len <= tryMax; len++)
            {
                var slice = target.AsSpan(i, len).ToString();
                if (!dict.Contains(slice)) continue;

                foreach (var combo in combosAtI)
                {
                    if (maxParts.HasValue && combo.Count + 1 > maxParts.Value) continue;

                    var next = new List<string>(combo.Count + 1);
                    next.AddRange(combo);
                    next.Add(slice);

                    int nextIndex = i + len;
                    dp[nextIndex] ??= new List<List<string>>();
                    dp[nextIndex].Add(next);
                }
            }
        }

        return dp[target.Length] ?? Enumerable.Empty<List<string>>();
    }
}
