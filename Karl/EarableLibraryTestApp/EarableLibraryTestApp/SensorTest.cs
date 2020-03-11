using EarableLibrary;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class SensorTest<SensorType, SensorValueType> : Test<IEarable> where SensorType : ISensor
	{
		private readonly ManualResetEventSlim _sampleReceived = new ManualResetEventSlim();
		private SensorValueType _lastSample;

		// TODO: Test reading after reconnection
		public async override Task Run(IEarable earable)
		{
			if (!earable.IsConnected()) await earable.ConnectAsync();

			SensorType sensor = earable.GetSensor<SensorType>();
			Assert.NotNull(sensor);
			Assert.True(sensor is SensorType);
			if (sensor is IReadableSensor<SensorValueType> readable)
			{
				var value = await readable.ReadAsync();
				Debug.WriteLine("Current value of {0} is {1}", sensor, value);
			}
			if (sensor is ISubscribableSensor<SensorValueType> subscribable)
			{
				_sampleReceived.Reset();
				subscribable.ValueChanged += OnValueChanged;
				await subscribable.StartSamplingAsync();
				Debug.WriteLine("Waiting for a value update from sensor {0}", sensor);
				_sampleReceived.Wait(TimeSpan.FromSeconds(10));
				await subscribable.StopSamplingAsync();
				subscribable.ValueChanged -= OnValueChanged;
				if (_sampleReceived.IsSet)
				{
					Debug.WriteLine("Received value {1} from sensor {0}", sensor, _lastSample);
				} else
				{
					Debug.WriteLine("A timeout occured while waiting for a sample update!");
				}
				Assert.True(_sampleReceived.IsSet);
			}
		}

		private void OnValueChanged(object sender, SensorValueType sample)
		{
			_lastSample = sample;
			_sampleReceived.Set();
		}
	}
}
