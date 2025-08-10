using WordCombos.Core.Interfaces;

namespace WordCombos.Core.Services;

public sealed class SegmentationStrategyProviderFactory : ISegmentationStrategyProviderFactory
{
    public ISegmentationStrategyProvider Create(ISegmentationStrategy withReuse, ISegmentationStrategy noReuse)
        => new SegmentationStrategyProvider(withReuse, noReuse);
}
