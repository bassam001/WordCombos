using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WordCombos.Core;
using WordCombos.Core.Interfaces;
using WordCombos.Core.Services;
using WordCombos.Core.Strategies;
using WordCombos.WpfApp.Services;

namespace WordCombos.WpfApp;

public partial class App : Application
{
    private IServiceProvider _provider = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        // Providers
        services.AddSingleton<ITargetSelector, TargetSelector>();
        services.AddSingleton<IMaxPartLengthProvider, MaxPartLengthProvider>();

        // Strategies
        services.AddSingleton<DfsReuseSegmentationStrategy>();
        services.AddSingleton<DfsNoReuseSegmentationStrategy>();
        services.AddSingleton<DpReuseSegmentationStrategy>();
        services.AddSingleton<DpNoReuseSegmentationStrategy>();


        // Providers en Factories
        services.AddSingleton<ISegmentationStrategyProviderFactory, SegmentationStrategyProviderFactory>();
        services.AddSingleton<ICombinationFinderFactory, CombinationFinderFactory>();


        // File Dialog
        services.AddSingleton<IFileDialogService, FileDialogService>();

        // Word repository factory
        services.AddSingleton<Func<string, bool, IWordRepository>>(
            sp => (path, ci) => new WordCombos.Adapters.FileWordRepository(path, ci)
        );

        // ViewModels & Views
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        _provider = services.BuildServiceProvider();

        var window = _provider.GetRequiredService<MainWindow>();
        window.Show();
    }

}
