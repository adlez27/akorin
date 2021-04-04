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
using System.Linq;

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
            status = "Not recording or playing.";
            selectedLineInit = false;
            newProject = false;

            fontSize = settings.FontSize;
            waveformColor = Color.FromName(settings.WaveformColor);

            waveform = ((Window)_view).Find<AvaPlot>("Waveform");
            waveform.Plot.YAxis.Grid(false);
            waveform.Plot.YAxis.Ticks(false);
            waveform.Plot.XAxis.Grid(false);
            waveform.Plot.XAxis.Ticks(false);
            waveform.Plot.Frame(false);
            waveform.Plot.Layout(25, 0, 0, 0, 0);

            SelectedLineIndex = settings.LastLine;

            if (SelectedLine.Audio.Data.Length > 0)
                SelectedLine.RaisePropertyChanged("Audio");

            ((Window)_view).Closing += OnClosingEventHandler;
        }

        private bool newProject;
        public void NewProject()
        {
            StopAudio();
            newProject = true;
            OpenSettings("files");
        }

        public async void OpenProject()
        {
            StopAudio();
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
                SelectedLineIndex = settings.LastLine;
                FontSize = settings.FontSize;
                WaveformColor = Color.FromName(settings.WaveformColor);
            }
        }

        public void OnClosingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Exit();
        }

        public async void Exit()
        {
            SelectedLine.Audio.Write();
            if (settings.ProjectFile == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Directory = settings.DestinationFolder;
                saveFileDialog.DefaultExtension = "arp";

                var arp = new FileDialogFilter();
                arp.Name = "Akorin recording project";
                arp.Extensions = new List<string>() { "arp" };
                saveFileDialog.Filters = new List<FileDialogFilter>() { arp };

                settings.ProjectFile = await saveFileDialog.ShowAsync((Window)_view);
            }
            settings.SaveSettings(settings.ProjectFile);
            Environment.Exit(0);
        }

        public void OpenSettings(string tab)
        {
            StopAudio();
            var settingsWindow = new SettingsWindow(tab, settings, this, newProject);
            settingsWindow.ShowDialog((Window)_view);
            newProject = false;
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
        private int selectedLineIndex;
        public int SelectedLineIndex
        {
            get => selectedLineIndex;
            set
            {
                if (RecList.Count > 0)
                {
                    if (selectedLineInit)
                    {
                        StopAudio();
                    }

                    this.RaiseAndSetIfChanged(ref selectedLineIndex, value);
                    this.RaisePropertyChanged("SelectedLine");

                    if (selectedLineInit)
                    {
                        SelectedLine.Audio.Read();
                        settings.LastLine = selectedLineIndex;

                        if (SelectedLine.Audio.Data.Length > 0)
                            SelectedLine.RaisePropertyChanged("Audio");
                    }
                    else
                    {
                        selectedLineInit = true;
                    }

                    ShowWaveform();
                    playToggle = false;
                    recordToggle = false;
                }
            }
        }

        public RecListItem SelectedLine
        {
            get => RecList[SelectedLineIndex];
        }

        public bool EditingNotes
        {
            get => ((Window)_view).FindControl<TextBox>("Notes").IsFocused;
        }

        private bool recordToggle;
        public void Record()
        {
            if (!EditingNotes)
            {
                if (playToggle)
                {
                    Play();
                }
                if (recordToggle)
                {
                    SelectedLine.Audio.Stop(); // change this
                    Status = "Not recording or playing.";
                    if (SelectedLine.Audio.Data.Length > 0) // change this
                        SelectedLine.RaisePropertyChanged("Audio");
                    ShowWaveform();
                }
                else
                {
                    SelectedLine.Audio.Record(); // change this
                    Status = "Recording...";
                }

                recordToggle = !recordToggle;
            }
        }

        private bool playToggle;
        public void Play()
        {
            if (!EditingNotes)
            {
                if (recordToggle)
                {
                    Record();
                }

                if (playToggle)
                {
                    SelectedLine.Audio.Stop();
                    Status = "Not recording or playing.";
                }
                else
                {
                    SelectedLine.Audio.Play();
                    Status = "Playing...";
                }

                playToggle = !playToggle;
            }
        }

        private void StopAudio()
        {
            if (playToggle || recordToggle)
            {
                SelectedLine.Audio.Stop();
                playToggle = false;
                recordToggle = false;
                Status = "Not recording or playing.";
            }
            SelectedLine.Audio.Write();
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref status, value);
            }
        }

        private Color waveformColor;
        public Color WaveformColor
        {
            get { return waveformColor; }
            set 
            {
                this.RaiseAndSetIfChanged(ref waveformColor, value);
                ShowWaveform();
            }
        }

        public void ShowWaveform()
        {
            waveform.Plot.Clear();
            waveform.Configuration.Pan = false;
            waveform.Configuration.Zoom = false;

            if (settings.WaveformEnabled)
            {
                double[] dataDouble;
                if (SelectedLine.Audio.Data.Length < 1)
                    dataDouble = new double[] { 0.0 };
                else
                    dataDouble = Array.ConvertAll(SelectedLine.Audio.Data, s => (double)s);

                var signalGraph = waveform.Plot.AddSignal(dataDouble, 44100, WaveformColor);
                waveform.Plot.Add(signalGraph);
                waveform.Plot.AxisAutoX(0);
                waveform.Plot.XAxis.Grid(true);
                waveform.Plot.XAxis.Ticks(true);
                waveform.Plot.YAxis.Ticks(true);

                var min = dataDouble.ToList().Min();
                var max = dataDouble.ToList().Max();
                var trueMax = Math.Max(0 - min, max);
                waveform.Plot.SetAxisLimitsY(0 - trueMax, trueMax);
            } else
            {
                waveform.Plot.XAxis.Grid(false);
                waveform.Plot.XAxis.Ticks(false);
                waveform.Plot.YAxis.Ticks(false);
            }

            waveform.Render();
        }
    }
}