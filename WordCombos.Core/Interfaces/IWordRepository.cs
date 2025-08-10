namespace WordCombos.Core.Interfaces;

public interface IWordRepository
{
    ISet<string> GetAllWords();
}
