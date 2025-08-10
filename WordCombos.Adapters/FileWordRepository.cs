using System.Text;
using WordCombos.Core.Interfaces;

namespace WordCombos.Adapters;

public sealed class FileWordRepository : IWordRepository
{
    private readonly string _path ;
    private readonly bool _caseInsensitive;

    public FileWordRepository(string path, bool caseInsensitive)
    {
        _path = path;
        _caseInsensitive = caseInsensitive;
    }

    public ISet<string> GetAllWords()
    {
        if (!File.Exists(_path))
            throw new FileNotFoundException($"Input file not found at: {_path}\nBase: {AppContext.BaseDirectory}");

        var set = new HashSet<string>(_caseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

        // Only UTF8 files !!
        foreach (var line in File.ReadLines(_path, Encoding.UTF8))
        {
            var w = line.Trim();
            if (w.Length == 0) continue;
            set.Add(_caseInsensitive ? w.ToLowerInvariant() : w);
        }

        return set;
    }
}
