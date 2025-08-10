using Microsoft.Win32;

namespace WordCombos.WpfApp.Services;

public sealed class FileDialogService : IFileDialogService
{
    public string? OpenTextFile()
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };
        return dlg.ShowDialog() == true ? dlg.FileName : null;
    }
}
