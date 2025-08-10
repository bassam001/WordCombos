using WordCombos.Core.Interfaces;

namespace WordCombos.Core.Services;

public sealed class SegmentationStrategyProvider : ISegmentationStrategyProvider
{
    private readonly ISegmentationStrategy _withReuse;
    private readonly ISegmentationStrategy _noReuse;

    public SegmentationStrategyProvider(ISegmentationStrategy withReuse, ISegmentationStrategy noReuse)
    {
        _withReuse = withReuse;
        _noReuse = noReuse;
    }

    public ISegmentationStrategy Get(bool allowReuse) => allowReuse ? _withReuse : _noReuse;
}
