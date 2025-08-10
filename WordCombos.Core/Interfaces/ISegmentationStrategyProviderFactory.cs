namespace WordCombos.Core.Interfaces;

public interface ISegmentationStrategyProviderFactory
{
    ISegmentationStrategyProvider Create(ISegmentationStrategy withReuse, ISegmentationStrategy noReuse);
}
