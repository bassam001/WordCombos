namespace WordCombos.Core.Interfaces;

public interface ITargetSelector
{
    IReadOnlyList<string> SelectTargets(ISet<string> words, int targetLength);
}
