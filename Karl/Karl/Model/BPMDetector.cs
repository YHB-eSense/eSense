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
		private static double BPM;
		private static double sampleRate = 44100;
		private static double SongLength = 0;
		private static short[] leftChannel;
		private static short[] rightChannel;
		private static short[] sampleBuffer;

		/// <summary>
		/// Used Method from http://mziccard.me/2015/05/28/beats-detection-algorithms-1/
		/// to detect BPM
		/// </summary>
		/// <param name="filename"> Path to File used for detection </param>
		/// <returns>BPM of song</returns>
		public static double DetectBPM(string filename)
		{
			MP3Stream stream = new MP3Stream(File.OpenRead(filename));
			int stepSize = 4096;
			byte[] buffer = new byte[stream.Length];
			byte[] bufferOfBuffer = new byte[stepSize];
			int read = 1;

			//Read MP3 File
			for (int i = 0; read > 0 && (i+1) * stepSize < stream.Length; i++) {
				read = stream.Read(bufferOfBuffer, 0,stepSize);
				for (int j = 0; j < read; j++ ) {
					buffer[j + (i * stepSize)] = bufferOfBuffer[j];
				}
			}
			
			//Get Values for left and right Channel
			sampleBuffer = new short[buffer.Length / 2];
			Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, buffer.Length / 2);
			leftChannel = getChannel(0,sampleBuffer);
			rightChannel = getChannel(1, sampleBuffer);
			stream.Close();

			//Compute Energy
			SongLength = (float)leftChannel.Length / sampleRate;
			int sampleStep = 4100;
			List<double> energies = new List<double>();
			for (int i = 0; i < leftChannel.Length - sampleStep - 1; i += sampleStep)
			{
				energies.Add(SumOfSquaredRange(leftChannel, i, i + sampleStep));
			}

			int beats = 0;
			double average = 0;
			double sumOfSquaresOfDifferences = 0;
			double variance = 0;
			double newC = 0;
			List<double> variances = new List<double>();
			int offset = 10;

			//Searching for Energy Peaks (Beats)
			for (int i = offset; i <= energies.Count - offset - 1; i++)
			{
				double currentEnergy = energies[i];
				double qwe = SumofRange(energies.ToArray(), i - offset, i - 1) + currentEnergy + SumofRange(energies.ToArray(), i + 1, i + offset);
				qwe /= offset * 2 + 1;
				// calculate local energy average and variance
				List<double> nearbyEnergies = energies.Skip(i - 5).Take(5).Concat(energies.Skip(i + 1).Take(5)).ToList<double>();
				average = nearbyEnergies.Average();
				sumOfSquaresOfDifferences = nearbyEnergies.Select(val => (val - average) * (val - average)).Sum();
				variance = (sumOfSquaresOfDifferences / nearbyEnergies.Count) / Math.Pow(10, 22);
				newC = variance * 0.009 + 1.385;
				// Check if current location in track is a beat
				if (currentEnergy > newC * qwe)
					beats++;
			}

			BPM = beats / (SongLength / 60);
			return BPM; 
		}


		/// <summary>
		/// Calculates Sum of squared elements of data between start and end
		/// </summary>
		/// <param name="data">Array with Elements</param>
		/// <param name="start">Starting Point</param>
		/// <param name="end">end Point</param>
		/// <returns>Sum of squared elements in range</returns>
		private static double SumOfSquaredRange(short[] data, int start, int end)
		{
			double result = 0;
			for (int i = start; i <= end; i++)
			{
				result += Math.Pow(data[i], 2);
			}

			return result;
		}

		/// <summary>
		/// Calculates Sum of elements of data between start and end
		/// </summary>
		/// <param name="data">Array with Elements</param>
		/// <param name="start">Starting Point</param>
		/// <param name="end">end Point</param>
		/// <returns>Sum of  elements in range</returns>
		private static double SumofRange(double[] data, int start, int stop)
		{
			double result = 0;
			for (int i = start; i <= stop; i++)
			{
				result += data[i];
			}
			return result;
		}

		/// <summary>
		/// Returns values of either right or left channel of samplesBuffer
		/// </summary>
		/// <param name="direction">for either left or right channel</param>
		/// <param name="samplesBuffer">given samples</param>
		/// <returns>values of left/right channel</returns>
		private static short[] getChannel(int direction, short[] samplesBuffer)
		{
			List<short> result = new List<short>();
			for (int i = 1; i + 1 < sampleBuffer.Length; i += 2)
			{
				if (direction == 0) result.Add(sampleBuffer[i - 1]);
				else result.Add(sampleBuffer[i]);
			}
			return result.ToArray();
		}


	}
}

