using EarableLibrary;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class SensorTest<SensorType, SensorValueType> : Test<IEarable> where SensorType : ISensor
	{
		private TaskCompletionSource<SensorValueType> _sampleReceiver;

		public SensorTest(IStatus status) : base(status) { }

		public async override Task Run(IEarable earable)
		{
			SensorType sensor = earable.GetSensor<SensorType>();
			Assert.NotNull(sensor);
			Assert.True(sensor is SensorType);
			if (sensor is IReadableSensor<SensorValueType> readable)
			{
				await TestReadableSensor(earable, readable);
			}
			if (sensor is ISubscribableSensor<SensorValueType> subscribable)
			{
				await TestSubscribableSensor(earable, subscribable);
			}
		}

		private async Task TestReadableSensor(IEarable earable, IReadableSensor<SensorValueType> sensor)
		{
			await EarableUtility.Reconnect(earable);
			Status.StatusUpdate("Reading value of sensor {0}", sensor);
			Status.StatusUpdate("Current value is {0}", await sensor.ReadAsync());
		}

		private async Task TestSubscribableSensor(IEarable earable, ISubscribableSensor<SensorValueType> sensor)
		{
			await EarableUtility.Reconnect(earable);
			_sampleReceiver = new TaskCompletionSource<SensorValueType>();
			sensor.ValueChanged += OnValueChanged;
			Status.StatusUpdate("Subscribing to sensor {0}", sensor);
			await sensor.StartSamplingAsync();
			Status.StatusUpdate("Waiting for a value update from sensor {0}", sensor);
			var valueReceived = _sampleReceiver.Task;
			var timeout = Task.Delay(10000);
			await Task.WhenAny(valueReceived, timeout);
			_sampleReceiver = null;
			await sensor.StopSamplingAsync();
			sensor.ValueChanged -= OnValueChanged;
			Assert.True(valueReceived.IsCompleted, "Result recieved within 10 seconds.");
			Status.StatusUpdate("Received value {1} from sensor {0}", sensor, valueReceived.Result);
		}


		private void OnValueChanged(object sender, SensorValueType sample)
		{
			_sampleReceiver?.SetResult(sample);
		}

		public override string ToString()
		{
			return string.Format("{0}<{1}>", base.ToString(), typeof(SensorType).Name);
		}
	}
}
