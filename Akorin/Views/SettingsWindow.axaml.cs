using Akorin.Models;
using Akorin.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace Akorin.Views
{
    public class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsWindowViewModel();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public SettingsWindow(string tab, ISettings s)
        {
            InitializeComponent();
            DataContext = new SettingsWindowViewModel(this, tab, s);

#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}