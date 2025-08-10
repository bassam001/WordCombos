using WordCombos.Core.Interfaces;
using WordCombos.Core.Strategies;

namespace WordCombos.Core.Services;

public sealed class CombinationFinderFactory : ICombinationFinderFactory
{
    private readonly ITargetSelector _targets;
    private readonly IMaxPartLengthProvider _maxLen;

    private readonly DfsReuseSegmentationStrategy _dfsReuse;
    private readonly DfsNoReuseSegmentationStrategy _dfsNoReuse;
    private readonly DpReuseSegmentationStrategy _dpReuse;
    private readonly DpNoReuseSegmentationStrategy _dpNoReuse;

    public CombinationFinderFactory(
        ITargetSelector targets,
        IMaxPartLengthProvider maxLen,
        DfsReuseSegmentationStrategy dfsReuse,
        DfsNoReuseSegmentationStrategy dfsNoReuse,
        DpReuseSegmentationStrategy dpReuse,
        DpNoReuseSegmentationStrategy dpNoReuse)
    {
        _targets = targets;
        _maxLen = maxLen;
        _dfsReuse = dfsReuse;
        _dfsNoReuse = dfsNoReuse;
        _dpReuse = dpReuse;
        _dpNoReuse = dpNoReuse;
    }

    public ICombinationFinder Create(SegmentationAlgorithm algorithm, bool allowReuse)
    {
        ISegmentationStrategy strategy = (algorithm, allowReuse) switch
        {
            (SegmentationAlgorithm.Dfs, true) => _dfsReuse,
            (SegmentationAlgorithm.Dfs, false) => _dfsNoReuse,
            (SegmentationAlgorithm.Dp, true) => _dpReuse,
            (SegmentationAlgorithm.Dp, false) => _dpNoReuse,
            _ => _dfsReuse
        };

        return new CombinationFinder(_targets, _maxLen, strategy);
    }
}
