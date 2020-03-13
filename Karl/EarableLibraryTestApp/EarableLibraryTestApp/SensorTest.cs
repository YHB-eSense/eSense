using EarableLibrary;
using System;
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
				Status.StatusUpdate("Current value of {0} is {1}", sensor, value);
			}
			if (sensor is ISubscribableSensor<SensorValueType> subscribable)
			{
				_sampleReceived.Reset();
				subscribable.ValueChanged += OnValueChanged;
				await subscribable.StartSamplingAsync();
				Status.StatusUpdate("Waiting for a value update from sensor {0}", sensor);
				_sampleReceived.Wait(TimeSpan.FromSeconds(10));
				await subscribable.StopSamplingAsync();
				subscribable.ValueChanged -= OnValueChanged;
				Assert.True(_sampleReceived.IsSet, "Sensor upate should be received within a maximum of 10 seconds.");
				Status.StatusUpdate("Received value {1} from sensor {0}", sensor, _lastSample);
			}
		}

		private void OnValueChanged(object sender, SensorValueType sample)
		{
			_lastSample = sample;
			_sampleReceived.Set();
		}
	}
}
