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
				Status.StatusUpdate("Reading value of sensor {0}", sensor);
				Status.StatusUpdate("Current value is {0}", await readable.ReadAsync());
				await Reconnect(earable);
				Status.StatusUpdate("Value after reconnection is {0}", await readable.ReadAsync());
			}
			if (sensor is ISubscribableSensor<SensorValueType> subscribable)
			{
				Status.StatusUpdate("Sensor {0} is subscribable", sensor);
				/*_sampleReceived.Reset();
				subscribable.ValueChanged += OnValueChanged;
				await subscribable.StartSamplingAsync();
				Status.StatusUpdate("Waiting for a value update from sensor {0}", sensor);
				_sampleReceived.Wait(TimeSpan.FromSeconds(10));
				await subscribable.StopSamplingAsync();
				subscribable.ValueChanged -= OnValueChanged;
				Assert.True(_sampleReceived.IsSet, "Sensor upate should be received within a maximum of 10 seconds.");
				Status.StatusUpdate("Received value {1} from sensor {0}", sensor, _lastSample);*/
			}
		}

		private void OnValueChanged(object sender, SensorValueType sample)
		{
			_lastSample = sample;
			_sampleReceived.Set();
		}

		private async Task Reconnect(IEarable earable)
		{
			Assert.True(await earable.DisconnectAsync());
			Assert.True(await earable.ConnectAsync());
		}
	}
}
