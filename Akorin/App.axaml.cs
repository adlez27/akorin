using Akorin.Models;
using Akorin.ViewModels;
using Akorin.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Akorin
{
    public class App : Application
    {
        public ISettings settings;
        public override void Initialize()
        {
            settings = new Settings(true);
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(settings);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}