using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Akorin.Models
{
    public class WaveformGenerator
    {
        private int sampleRate;
        private List<double> averageSamplesL;
        private List<double> averageSamplesR;

        public WaveformGenerator()
        {
            sampleRate = 8820; //0.2s per sample
            averageSamplesL = new List<double>();
            averageSamplesR = new List<double>();
        }

        public void DrawWaveform(short[] data)
        {
            if (data.Length == 0) return;

            int halvedSampleRate = sampleRate / 2;
            short[] dataL = new short[data.Length / 2];
            short[] dataR = new short[data.Length / 2];
            double sampleStart;
            double sampleEnd;
            double tempSum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (i % 2 == 0)
                {
                    dataL.Append(data[i]);
                }
                else
                {
                    dataR.Append(data[i]);
                }
            }

            for (int i = 0; i < Math.Ceiling((double) dataL.Length / halvedSampleRate); i++)
            {
                if (((i + 1) * halvedSampleRate) > dataL.Length)
                {
                    sampleStart = i * halvedSampleRate;
                    sampleEnd = dataL.Length;
                    for (int j = (int)sampleStart; sampleStart <= sampleEnd; sampleStart++)
                    {
                        tempSum += dataL[j];
                    }
                }
                else
                {
                    sampleStart = i * halvedSampleRate;
                    sampleEnd = (i + 1) * halvedSampleRate;
                    for (int j = (int) sampleStart; sampleStart <= sampleEnd; sampleStart++)
                    {
                        tempSum += dataL[j];
                    }
                }
                averageSamplesL.Add(Math.Round(tempSum / (sampleEnd - sampleStart)));
                tempSum = 0;
            }

            for (int i = 0; i < Math.Ceiling((double) dataR.Length / halvedSampleRate); i++)
            {
                if (((i + 1) * halvedSampleRate) > dataR.Length)
                {
                    sampleStart = i * halvedSampleRate;
                    sampleEnd = dataR.Length;
                    for (int j = (int)sampleStart; sampleStart <= sampleEnd; sampleStart++)
                    {
                        tempSum += dataR[j];
                    }
                }
                else
                {
                    sampleStart = i * halvedSampleRate;
                    sampleEnd = (i + 1) * halvedSampleRate;
                    for (int j = (int)sampleStart; sampleStart <= sampleEnd; sampleStart++)
                    {
                        tempSum += dataR[j];
                    }
                }
                averageSamplesR.Add(Math.Round(tempSum / (sampleEnd - sampleStart)));
                tempSum = 0;
            }

            Debug.WriteLine(data.Length);
        }

        public void ClearWaveform()
        {

        }
    }
}
