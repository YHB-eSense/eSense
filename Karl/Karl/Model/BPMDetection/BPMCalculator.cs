using NAudio.Wave;
using SoundTouch;
using System.Collections.Generic;
using System.Threading;

namespace Karl.Model
{
	public class BPMCalculator
	{
		private string _file;
		private WaveFileReader _reader;
		private int _chunkSize;
		private long _sampleCount;
		private long _sampleCountFraction;
		BpmDetect _detector;
		int _bpmMax;
		int _bpmMin;

		public BPMCalculator(string file)
		{
			_file = file;
			_reader = new WaveFileReader(_file);
			_chunkSize = 5000;
			_sampleCount = _reader.SampleCount;
			_sampleCountFraction = _sampleCount / _chunkSize;
			_detector = new BpmDetect(1, 44100);
			_bpmMax = 180;
			_bpmMin = 70;
		}
		public int Calculate()
		{
			if (_file != null)
			{
				for(int n = 1; n < _chunkSize; n++)
				{
					List<float> samples = new List<float>();
					for (long i = 0; i < _sampleCountFraction; i++)
					{
						var sampleFrame = _reader.ReadNextSampleFrame();
						if (sampleFrame == null) { break; }
						samples.Add(sampleFrame[0]);
						samples.Add(sampleFrame[1]);
					}
					_detector.InputSamples(samples.ToArray(), samples.Count);
					samples.Clear();
					System.Diagnostics.Debug.WriteLine(n + "/" + _chunkSize);
				}
				float bpm = _detector.GetBpm();
				if (bpm > _bpmMax) { bpm = bpm / 2; }
				else if (bpm < _bpmMin) { bpm = bpm * 2; }
				return (int)bpm;
			}
			return 0;
			/*
			using (WaveFileReader reader = new WaveFileReader(file))
			{
				long sampleCount = reader.SampleCount;
				BpmDetect detector = new BpmDetect(1, 22050);
				for (int i = 0; i < sampleCount; i++)
				{
					var sampleFrame = reader.ReadNextSampleFrame();
					if (sampleFrame == null) { break; }
					detector.InputSamples(sampleFrame, 2);
					System.Diagnostics.Debug.WriteLine( i / sampleCount + "%" );
				}
				return (int)detector.GetBpm();
			}
			*/
		}
	}
}
