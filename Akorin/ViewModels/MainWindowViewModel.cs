using Akorin.Models;
using Akorin.Views;
using Avalonia.Controls;
using Avalonia.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Akorin.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IView _view;
        private ISettings settings;
        public MainWindowViewModel() { }

        public MainWindowViewModel(IView view, ISettings s)
        {
            _view = view;
            settings = s;
            playToggle = false;
            recordToggle = false;
            recordPlayStatus = "Not recording or playing.";
            selectedLineInit = false;

            if (RecList[0].Audio.Data.Length > 0)
                FileStatus = "Audio available";
            else
                FileStatus = "No audio";
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void OpenSettings(string tab)
        {
            var settingsWindow = new SettingsWindow(tab, settings);
            settingsWindow.ShowDialog((Window)_view);
        }

        public int FontSize
        {
            get { return settings.FontSize; }
        }

        public ObservableCollection<RecListItem> RecList
        {
            get
            {
                return settings.RecList;
            }
        }

        private bool selectedLineInit;
        private RecListItem selectedLine;
        public RecListItem SelectedLine
        {
            get => selectedLine;
            set
            {
                if (selectedLineInit)
                {
                    if (playToggle || recordToggle)
                    {
                        selectedLine.Audio.Stop();
                        playToggle = false;
                        recordToggle = false;
                        RecordPlayStatus = "Not recording or playing.";
                    }
                    selectedLine.Audio.Write();

                    if (selectedLine.Audio.Data.Length > 0)
                        FileStatus = "Audio available";
                    else
                        FileStatus = "No audio";
                }
                else
                {
                    selectedLineInit = true;
                }

                this.RaiseAndSetIfChanged(ref selectedLine, value);
                playToggle = false;
                recordToggle = false;
            }
        }

        private bool recordToggle;
        public void Record()
        {
            if (playToggle)
            {
                Play();
            }
            if (recordToggle)
            {
                SelectedLine.Audio.Stop();
                RecordPlayStatus = "Not recording or playing.";
            }
            else
            {
                SelectedLine.Audio.Record();
                RecordPlayStatus = "Recording...";
            }

            recordToggle = !recordToggle;
        }

        private bool playToggle;
        public void Play()
        {
            if (recordToggle)
            {
                Record();
            }

            if (playToggle)
            {
                SelectedLine.Audio.Stop();
                RecordPlayStatus = "Not recording or playing.";
            }
            else
            {
                SelectedLine.Audio.Play();
                RecordPlayStatus = "Playing...";
            }

            playToggle = !playToggle;
        }

        private string recordPlayStatus;
        public string RecordPlayStatus
        {
            get
            {
                return recordPlayStatus;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref recordPlayStatus, value);
            }
        }

        private string fileStatus;
        public string FileStatus
        {
            get
            {
                return fileStatus;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref fileStatus, value);
            }
        }
    }
}