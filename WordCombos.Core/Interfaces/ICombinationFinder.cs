using WordCombos.Core.Models;

namespace WordCombos.Core.Interfaces;

public interface ICombinationFinder
{
    IEnumerable<Combination> FindAll(
        ISet<string> words,
        int targetLength,
        int minParts = 2,
        int? maxParts = null,
        bool allowReuse = true);
}

//default parameter values in interfaces 
//    are allowed since C# 9, 
//    and they are useful because they let you define flexible method contracts 
//    without repeating the same defaults in every implementation.