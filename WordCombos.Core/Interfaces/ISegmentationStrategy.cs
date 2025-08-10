namespace WordCombos.Core.Interfaces;

public interface ISegmentationStrategy
{
    IEnumerable<IReadOnlyList<string>> Segment(string target, ISet<string> dict, int maxPartLen, int? maxParts);
}
