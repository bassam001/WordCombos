using Moq;
using WordCombos.Core;
using WordCombos.Core.Interfaces;

namespace WordCombos.Tests;

public sealed class CombinationFinderFilteringTests
{
    [Fact]
    public void FindAll_Should_OnlyReturnCombinations_WithAtLeastMinParts()
    {

        var words = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "target", "a", "b", "c"
    };

        var targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .Setup(s => s.SelectTargets(It.IsAny<ISet<string>>(), 6))
            .Returns(new[] { "target" });

        var maxLen = new Mock<IMaxPartLengthProvider>();
        maxLen
            .Setup(m => m.Get(It.IsAny<ISet<string>>(), 6))
            .Returns(6);

        var strategy = new Mock<ISegmentationStrategy>();
        strategy
            .Setup(s => s.Segment("target", It.IsAny<ISet<string>>(), 6, It.IsAny<int?>()))
            .Returns(new[]
            {
            new[] { "a", "b" },       
            new[] { "a", "b", "c" }   
            });

        var sut = new CombinationFinder(targetSelector.Object, maxLen.Object, strategy.Object);

        // Act
        var results = sut
            .FindAll(words, 6, minParts: 3, maxParts: null)
            .Select(r => r.Parts.Count)
            .ToArray();

        // Assert
        Assert.NotEmpty(results);
        Assert.All(results, count => Assert.True(count >= 3));
        Assert.DoesNotContain(2, results);
    }


    [Fact]
    public void FindAll_Should_Pass_MaxParts_And_MaxPartLen_ToSegmentationStrategy()
    {
        // Arrange
        var words = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "abc", "a", "b", "c"
    };

        var targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .Setup(s => s.SelectTargets(It.IsAny<ISet<string>>(), 3))
            .Returns(new[] { "abc" });

        var maxLen = new Mock<IMaxPartLengthProvider>();
        maxLen
            .Setup(m => m.Get(It.IsAny<ISet<string>>(), 3))
            .Returns(3);

        var capturedMaxParts = new List<int?>();
        var capturedMaxPartLens = new List<int>();

        var strategy = new Mock<ISegmentationStrategy>();
        strategy
            .Setup(s => s.Segment(
                "abc",
                It.IsAny<ISet<string>>(),
                It.IsAny<int>(),
                It.IsAny<int?>()))
            .Callback<string, ISet<string>, int, int?>((_, __, mxLen, mx) =>
            {
                capturedMaxPartLens.Add(mxLen);
                capturedMaxParts.Add(mx);
            })
            .Returns(new[] { (IReadOnlyList<string>)new[] { "a", "b", "c" } });

        var sut = new CombinationFinder(targetSelector.Object, maxLen.Object, strategy.Object);

        // Act
        var results = sut.FindAll(words, 3, minParts: 2, maxParts: 2).ToList();

        // Assert
        Assert.NotEmpty(capturedMaxParts);
        Assert.Contains(2, capturedMaxParts);                    
        Assert.All(capturedMaxPartLens, l => Assert.Equal(3, l));  
        Assert.NotEmpty(results);
    }

}
