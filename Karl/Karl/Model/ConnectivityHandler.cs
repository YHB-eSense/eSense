using System;
using EarableLibrary;
using System.Diagnostics;
using System.Threading.Tasks;
using StepDetectionLibrary;
using static Karl.Model.AudioPlayer;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the conntection to the Earables Library and Step Detection Library
	/// </summary>
	public class ConnectivityHandler
	{

		private static ConnectivityHandler _connectivityHandler;

		/// <summary>
		/// 
		/// </summary>
		public static ConnectivityHandler SingletonConnectivityHandler
		{
			get
			{
				if (_connectivityHandler == null)
				{
					_connectivityHandler = new ConnectivityHandler();
				}
				return _connectivityHandler;
			}
		}

		private readonly IEarableManager _earableManager;
		private readonly StepDetectionLibrary.Input _stepDetection;
		private IEarable _connectedEarable;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		public delegate void ConnectionEventHandler(object source, EventArgs e);

		/// <summary>
		/// 
		/// </summary>
		public event ConnectionEventHandler ConnectionChanged;


		private ConnectivityHandler()
		{
			_earableManager = new EarableLibrary.EarableLibrary();
			_stepDetection = new Input();
		}

		/// <summary>
		/// 
		/// </summary>
		public bool EarableConnected
		{
			get
			{
				if (_connectedEarable == null) return false;
				if (_connectedEarable.State == ConnectionState.Disconnected) _connectedEarable = null;
				return _connectedEarable.State == ConnectionState.Connected;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string EarableName
		{
			get => _connectedEarable.Name;
		}

		/// <summary>
		/// Tries establishing a BLE connection to a bonded EarableDevice.
		/// </summary>
		/// <returns>null if connection failed, the newly connected device otherwise</returns>
		public async Task<bool> Connect()
		{
			_connectedEarable = await _earableManager.ConnectEarableAsync();

			if (_connectedEarable == null) return false;

			/*var imu = _connectedEarable.GetSensor<MotionSensor>();
			imu.SamplingRate = _stepDetection.SamplingRate;
			imu.ValueChanged += _stepDetection.ValueChanged;
			imu.StartSampling();*/

			var button = _connectedEarable.GetSensor<PushButton>();
			button.ValueChanged += (s, args) =>
			{
				Debug.WriteLine(args.Pressed ? "Pushed :)" : "Released :(");
				bool released = !args.Pressed;
				if (released) SingletonAudioPlayer.TogglePause();
			};
			button.StartSampling();

			ConnectionChanged?.Invoke(this, null);

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task Disconnect()
		{
			if (!EarableConnected) return;
			await _connectedEarable.DisconnectAsync();
			_connectedEarable = null;
			ConnectionChanged?.Invoke(this, null);
		}

		/// <summary>
		/// Set a new Device name.
		/// </summary>
		/// <param name="name">The new Name.</param>
		public async Task SetDeviceNameAsync(string name)
		{
			if (!EarableConnected) return;
			_connectedEarable.Name = name;
		}

	}
}
