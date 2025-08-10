using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WordCombos.Core.Interfaces;
using WordCombos.Core.Strategies;
using WordCombos.WpfApp.Services;

namespace WordCombos.WpfApp;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly ICombinationFinderFactory _finderFactory;
    private readonly IFileDialogService _fileDialog;
    private readonly Func<string, bool, IWordRepository> _repoFactory;

    private string _inputPath = "input.txt";
    private int _targetLength = 6;
    private int _minParts = 2;
    private int? _maxParts;
    private bool _allowReuse = true;
    private bool _caseInsensitive = true;
    private string _status = "";
    private SegmentationAlgorithm _selectedAlgorithm = SegmentationAlgorithm.Dfs;

    public MainViewModel(
        ICombinationFinderFactory finderFactory,
        IFileDialogService fileDialog,
        Func<string, bool, IWordRepository> repoFactory)
    {
        _finderFactory = finderFactory;
        _fileDialog = fileDialog;
        _repoFactory = repoFactory;

        Results = new ObservableCollection<string>();
        Algorithms = new[] { SegmentationAlgorithm.Dfs, SegmentationAlgorithm.Dp };
        BrowseCommand = new RelayCommand(Browse);
        RunCommand = new RelayCommand(Run, CanRun);
    }

    public ObservableCollection<string> Results { get; }
    public Array Algorithms { get; }
    public RelayCommand BrowseCommand { get; }
    public RelayCommand RunCommand { get; }

    public SegmentationAlgorithm SelectedAlgorithm
    {
        get => _selectedAlgorithm;
        set { _selectedAlgorithm = value; OnPropertyChanged(); }
    }

    public string InputPath { get => _inputPath; set { _inputPath = value; OnPropertyChanged(); RunCommand.RaiseCanExecuteChanged(); } }
    public int TargetLength { get => _targetLength; set { _targetLength = value; OnPropertyChanged(); } }
    public int MinParts { get => _minParts; set { _minParts = value; OnPropertyChanged(); } }
    public int? MaxParts { get => _maxParts; set { _maxParts = value; OnPropertyChanged(); } }
    public bool AllowReuse { get => _allowReuse; set { _allowReuse = value; OnPropertyChanged(); } }
    public bool CaseInsensitive { get => _caseInsensitive; set { _caseInsensitive = value; OnPropertyChanged(); } }
    public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }

    private void Browse()
    {
        var path = _fileDialog.OpenTextFile();
        if (!string.IsNullOrWhiteSpace(path)) InputPath = path;
    }

    private bool CanRun() => !string.IsNullOrWhiteSpace(InputPath);

    private void Run()
    {
        Results.Clear();
        try
        {
            var repo = _repoFactory(InputPath, CaseInsensitive);
            var words = repo.GetAllWords();

            var finder = _finderFactory.Create(SelectedAlgorithm, AllowReuse);

            var items = finder.FindAll(words, TargetLength, MinParts, MaxParts)
                              .OrderBy(r => r.Target)
                              .ThenBy(r => r.Parts.Count)
                              .Select(r => $"{string.Join('+', r.Parts)}={r.Target}")
                              .ToArray();

            foreach (var s in items) Results.Add(s);

            var algo = SelectedAlgorithm == SegmentationAlgorithm.Dp ? "DP" : "DFS";
            var reuse = AllowReuse ? "ON" : "OFF";
            Status = items.Length == 0

                //ulgy strings interpolation, but it works :))
                ? $"No combinations found. [{algo} | AllowReuse={reuse}]"
                : $"Found {items.Length} result(s). [{algo} | AllowReuse={reuse}]";
        }
        catch (Exception ex)
        {
            Status = ex.Message;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
