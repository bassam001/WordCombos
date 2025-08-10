using WordCombos.Core.Strategies;
namespace WordCombos.Core.Interfaces;

public interface ICombinationFinderFactory
{
    ICombinationFinder Create(SegmentationAlgorithm algorithm, bool allowReuse);
}
