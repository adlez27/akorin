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
        private List<short> averageSamplesL;
        private List<short> averageSamplesR;

        public WaveformGenerator()
        {
            sampleRate = 8820; //0.2s per sample
            averageSamplesL = new List<short>();
            averageSamplesR = new List<short>();
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
                if (((i + 1) * sampleRate) > dataL.Length)
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
                Debug.WriteLine(tempSum);
                averageSamplesL.Add((short) Math.Round(tempSum / (sampleEnd - sampleStart)));
                tempSum = 0;
            }

            for (int i = 0; i < Math.Floor((double) dataR.Length / halvedSampleRate); i++)
            {
                if (((i + 1) * sampleRate) > dataR.Length)
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
                Debug.WriteLine(tempSum);
                averageSamplesR.Add((short)Math.Round(tempSum / (sampleEnd - sampleStart)));
                tempSum = 0;
            }

            Debug.WriteLine(data.Length);
        }

        public void ClearWaveform()
        {

        }
    }
}
