using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace Akorin.Models
{
    public class WaveformGenerator
    {
        private SignalPlot waveformPlot;

        public WaveformGenerator()
        {
            waveformPlot = new SignalPlot();
        }

        public SignalPlot DrawWaveform(short[] data)
        {
            double[] dataDouble = Array.ConvertAll(data, s => (double) s);
            waveformPlot = new SignalPlot() { Ys = dataDouble, SampleRate = 44100, Color = Color.Blue };
            waveformPlot.FillType = FillType.FillBelow;
            waveformPlot.FillColor1 = Color.Blue;
            return waveformPlot;
        }

        public void ClearWaveform()
        {
            waveformPlot = new SignalPlot();
        }
    }
}
