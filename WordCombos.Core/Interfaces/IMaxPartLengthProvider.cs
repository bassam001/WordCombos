namespace WordCombos.Core.Interfaces;

public interface IMaxPartLengthProvider
{
    int Get(ISet<string> words, int targetLength);
}
