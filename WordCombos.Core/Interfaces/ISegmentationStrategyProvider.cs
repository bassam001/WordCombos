namespace WordCombos.Core.Interfaces;

public interface ISegmentationStrategyProvider
{
    ISegmentationStrategy Get(bool allowReuse);
}
