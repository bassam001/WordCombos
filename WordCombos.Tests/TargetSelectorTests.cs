using WordCombos.Core.Interfaces;
using WordCombos.Core.Strategies;


namespace WordCombos.Tests;

public sealed class TargetSelectorTests
{
    [Fact]
    public void SelectTargets_Returns_Words_With_Exact_Length()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "a","ab","abc","foobar","abcdef","FOOBAR" };

        ITargetSelector selector = new TargetSelector();
        var sut = selector.SelectTargets(set, 6);

        Assert.Contains("foobar", sut, StringComparer.OrdinalIgnoreCase);
        Assert.Contains("abcdef", sut, StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("abc", sut, StringComparer.OrdinalIgnoreCase);
        Assert.Equal(2, sut.Count);
    }

    [Fact]
    public void SelectTargets_Empty_When_None_Matches()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "a", "ab", "abc" };
        ITargetSelector selector = new TargetSelector();

        var sut = selector.SelectTargets(set, 6);

        Assert.Empty(sut);
    }
}
