using WordCombos.Core.Interfaces;

public sealed class DfsReuseSegmentationStrategy : ISegmentationStrategy
{
    public IEnumerable<IReadOnlyList<string>> Segment(
        string target, ISet<string> dict, int maxPartLen, int? maxParts)
    {
        var memo = new Dictionary<(int i, int? left), List<List<string>>>();

        List<List<string>> Dfs(int i, int? left)
        {
            if (i == target.Length) return new List<List<string>> { new List<string>() };
            if (left.HasValue && left.Value == 0) return new List<List<string>>();

            var key = (i, left);
            if (memo.TryGetValue(key, out var cached)) return cached;

            var res = new List<List<string>>();
            int remaining = target.Length - i;
            int tryMax = maxPartLen < remaining ? maxPartLen : remaining;

            for (int len = 1; len <= tryMax; len++)
            {
                var slice = target.AsSpan(i, len).ToString();
                if (!dict.Contains(slice)) continue;

                int? nextLeft = left.HasValue ? left.Value - 1 : null;
                var tails = Dfs(i + len, nextLeft);

                foreach (var t in tails)
                {
                    var combo = new List<string>(t.Count + 1) { slice };
                    combo.AddRange(t);
                    res.Add(combo);
                }
            }
            memo[key] = res;
            return res;
        }

        foreach (var r in Dfs(0, maxParts))
            yield return r;
    }
}
