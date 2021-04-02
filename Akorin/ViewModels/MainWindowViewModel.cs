using Avalonia.Controls;
using Avalonia.Input;
using Akorin.Models;
using Akorin.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Drawing;
using ScottPlot.Avalonia;
using ScottPlot;
using System.IO;

namespace Akorin.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IView _view;
        private ISettings settings;
        private AvaPlot waveform;
        public MainWindowViewModel() { }

        public MainWindowViewModel(IView view, ISettings s)
        {
            _view = view;
            settings = s;
            playToggle = false;
            recordToggle = false;
            recordPlayStatus = "Not recording or playing.";
            selectedLineInit = false;
            fontSize = settings.FontSize;

            ((Window)_view).Closing += OnClosingEventHandler;

            waveform = ((Window)_view).Find<AvaPlot>("Waveform");
            waveform.Plot.YAxis.Grid(false);
            waveform.Plot.YAxis.Ticks(false);
            waveform.Plot.XAxis.Grid(false);
            waveform.Plot.XAxis.Ticks(false);
            waveform.Plot.Frame(false);
            waveform.Plot.Layout(25, 0, 0, 0, 0);

            if (RecList[0].Audio.Data.Length > 0)
            {
                FileStatus = "Audio available";
                RecList[0].RaisePropertyChanged("Audio");
            }
            else
                FileStatus = "No audio";
        }

        public void OnClosingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            selectedLine.Audio.Write();
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void NewProject()
        {
            settings.LoadDefault();
            OpenSettings("files");
        }

        public async void OpenProject()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AllowMultiple = false;

            var filter = new FileDialogFilter();
            filter.Name = "Akorin recording project";
            filter.Extensions = new List<string>() { "arp" };
            openFileDialog.Filters = new List<FileDialogFilter>() { filter };

            openFileDialog.Directory = Path.GetDirectoryName(settings.ProjectFile);
            var projectFile = await openFileDialog.ShowAsync((Window)_view);
            if (projectFile.Length > 0)
            {
                settings.LoadSettings(projectFile[0]);
                FontSize = settings.FontSize;
            }
        }

        public void OpenSettings(string tab)
        {
            StopAudio();
            var settingsWindow = new SettingsWindow(tab, settings, this);
            settingsWindow.ShowDialog((Window)_view);
        }

        private int fontSize;
        public int FontSize
        {
            get { return fontSize; }
            set { this.RaiseAndSetIfChanged(ref fontSize, value); }
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
                if (RecList.Count > 0)
                {
                    if (selectedLineInit)
                    {
                        StopAudio();
                    }

                    this.RaiseAndSetIfChanged(ref selectedLine, value);

                    if (selectedLineInit)
                    {
                        selectedLine.Audio.Read();

                        if (selectedLine.Audio.Data.Length > 0)
                        {
                            FileStatus = "Audio available";
                            selectedLine.RaisePropertyChanged("Audio");
                        }
                        else
                            FileStatus = "No audio";
                    }
                    else
                    {
                        selectedLineInit = true;
                    }

                    playToggle = false;
                    recordToggle = false;
                }
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
                if (SelectedLine.Audio.Data.Length > 0)
                {
                    FileStatus = "Audio available";
                    selectedLine.RaisePropertyChanged("Audio");
                }
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

        private void StopAudio()
        {
            if (playToggle || recordToggle)
            {
                selectedLine.Audio.Stop();
                playToggle = false;
                recordToggle = false;
                RecordPlayStatus = "Not recording or playing.";
            }
            selectedLine.Audio.Write();
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
                if (value == "Audio available") ShowWaveform();
                this.RaiseAndSetIfChanged(ref fileStatus, value);
            }
        }

        public void ShowWaveform()
        {
            waveform.Plot.Clear();

            if (settings.WaveformEnabled)
            {
                double[] dataDouble = Array.ConvertAll(SelectedLine.Audio.Data, s => (double)s);
                var signalGraph = waveform.Plot.AddSignal(dataDouble, 44100, Color.Blue);
                waveform.Plot.Add(signalGraph);
                waveform.Plot.AxisAutoX(0);
                waveform.Plot.XAxis.Grid(true);
                waveform.Plot.YAxis.Ticks(true);
                waveform.Plot.SetAxisLimitsY(-10000, 10000);
            } else
            {
                waveform.Plot.XAxis.Grid(false);
                waveform.Plot.YAxis.Ticks(false);
            }

            waveform.Render();
        }
    }
}