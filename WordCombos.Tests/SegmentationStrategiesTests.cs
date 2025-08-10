using Moq;
using WordCombos.Core;
using WordCombos.Core.Interfaces;

namespace WordCombos.Tests;

public sealed class SegmentationStrategiesTests
{
    private const int TargetLength = 6;

    private static CombinationFinder CreateFinder(
        string target,
        int maxPartLen,
        ISegmentationStrategy strategy,
        out HashSet<string> words)
    {
        words = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var targetSelector = new Mock<ITargetSelector>();
        targetSelector.Setup(s => s.SelectTargets(It.IsAny<ISet<string>>(), TargetLength))
                      .Returns(new[] { target });

        var maxLen = new Mock<IMaxPartLengthProvider>();
        maxLen.Setup(m => m.Get(It.IsAny<ISet<string>>(), TargetLength))
              .Returns(maxPartLen);

        return new CombinationFinder(targetSelector.Object, maxLen.Object, strategy);
    }

    [Fact]
    public void DfsReuseSegmentationStrategy_Allows_Repeating_Words()
    {
        var strategy = new DfsReuseSegmentationStrategy();
        var finder = CreateFinder("aaaa", maxPartLen: 2, strategy, out var words);
        words.UnionWith(new[] { "a", "aa", "aaaa" });

        var results = finder.FindAll(words, TargetLength, minParts: 2, maxParts: null)
                            .Select(r => r.Parts.ToArray())
                            .ToList();

        Assert.Contains(results, p => string.Join("+", p).Equals("aa+aa", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(results, p => string.Join("+", p).Equals("a+a+a+a", StringComparison.OrdinalIgnoreCase));
        Assert.True(results.Count >= 2);
    }

    [Fact]
    public void DfsNoReuseSegmentationStrategy_Disallows_Duplicate_Words_In_Combinations()
    {
        var strategy = new DfsNoReuseSegmentationStrategy();
        var finder = CreateFinder("abab", maxPartLen: 2, strategy, out var words);
        words.UnionWith(new[] { "a", "b", "ab" });

        var results = finder.FindAll(words, TargetLength, minParts: 2, maxParts: null)
                            .Select(r => r.Parts.ToArray())
                            .ToList();

        Assert.NotEmpty(results);
        Assert.Contains(results, p => string.Join("+", p).Equals("ab+a+b", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(results, p =>
            p.GroupBy(x => x, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1));
    }


    [Fact]
    public void DpNoReuseSegmentationStrategy_Finds_Unique_Words_Once()
    {
        var strategy = new DpNoReuseSegmentationStrategy();
        var finder = CreateFinder("catdog", maxPartLen: 3, strategy, out var words);
        words.UnionWith(new[] { "cat", "dog", "at", "do" });

        var results = finder.FindAll(words, TargetLength, minParts: 2, maxParts: null)
                            .Select(r => string.Join("+", r.Parts))
                            .ToList();

        Assert.Contains("cat+dog", results);
        Assert.DoesNotContain(results, s => s.Contains("cat+cat", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(results, s => s.Contains("dog+dog", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void DpReuseSegmentationStrategy_Allows_Repetition_And_Produces_Multiple_Solutions()
    {
        var strategy = new DpReuseSegmentationStrategy();
        var finder = CreateFinder("aaaa", maxPartLen: 2, strategy, out var words);
        words.UnionWith(new[] { "a", "aa" });

        var results = finder.FindAll(words, TargetLength, minParts: 2, maxParts: null)
                            .Select(r => string.Join("+", r.Parts))
                            .ToList();

        Assert.Contains("aa+aa", results);
        Assert.Contains("a+a+a+a", results);
        Assert.True(results.Count >= 2);
    }
}
