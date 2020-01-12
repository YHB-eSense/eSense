using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NAudio.Wave;
using MP3Sharp;
using System.Reflection;

namespace Karl.Model
{
	static class BPMDectetor
	{
		private static short[] leftChn;
		private static short[] rightChn;
		private static double BPM;
		private static double sampleRate = 44100;
		private static double trackLength = 0;

		public static double DetectBPM(string filename)
		{
			MP3Stream stream = new MP3Stream(File.OpenRead(filename));
			int bsize = (int)stream.Length / 100;
			byte[] buffer = new byte[(int)stream.Length];
			byte[] bufferer = new byte[bsize];
			List<short> chan1 = new List<short>();
			List<short> chan2 = new List<short>();
			int read = 1;
			for (int i = 0; i < 100; i++) {
				read += stream.Read(bufferer, 0,bsize);
				for (int j = 0; j < bsize; j++ ) {
					buffer[j+(i*bsize)] = bufferer[j];
				}
			}
			short[] sampleBuffer = new short[buffer.Length / 2];
			Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, buffer.Length/2);
			for (int i = 1; i+1 < sampleBuffer.Length; i += 2)
			{
				chan1.Add(sampleBuffer[i-1]);
				chan2.Add(sampleBuffer[i]);
			}				
			leftChn = chan1.ToArray();
			rightChn = chan2.ToArray();
			stream.Close();
			trackLength = (float)leftChn.Length / sampleRate;
			int sampleStep = 3600;
			List<double> energies = new List<double>();
			for (int i = 0; i < leftChn.Length - sampleStep - 1; i += sampleStep)
			{
				energies.Add(rangeQuadSum(leftChn, i, i + sampleStep));
			}
			int beats = 0;
			double average = 0;
			double sumOfSquaresOfDifferences = 0;
			double variance = 0;
			double newC = 0;
			List<double> variances = new List<double>();
			int offset = 10;
			for (int i = offset; i <= energies.Count - offset - 1; i++)
			{
				// calculate local energy average
				double currentEnergy = energies[i];
				double qwe = rangeSum(energies.ToArray(), i - offset, i - 1) + currentEnergy + rangeSum(energies.ToArray(), i + 1, i + offset);
				qwe /= offset * 2 + 1;

				// calculate energy variance of nearby energies
				List<double> nearbyEnergies = energies.Skip(i - 5).Take(5).Concat(energies.Skip(i + 1).Take(5)).ToList<double>();
				average = nearbyEnergies.Average();
				sumOfSquaresOfDifferences = nearbyEnergies.Select(val => (val - average) * (val - average)).Sum();
				variance = (sumOfSquaresOfDifferences / nearbyEnergies.Count) / Math.Pow(10, 22);

				// experimental linear regression - constant calculated according to local energy variance
				newC = variance * 0.009 + 1.385;
				if (currentEnergy > newC * qwe)
					beats++;
			}
			BPM = beats / (trackLength / 60);
			return BPM; 
		}

		private static double rangeQuadSum(short[] samples, int start, int stop)
		{
			double tmp = 0;
			for (int i = start; i <= stop; i++)
			{
				tmp += Math.Pow(samples[i], 2);
			}

			return tmp;
		}

		private static double rangeSum(double[] data, int start, int stop)
		{
			double tmp = 0;
			for (int i = start; i <= stop; i++)
			{
				tmp += data[i];
			}
			return tmp;
		}
	}
}

