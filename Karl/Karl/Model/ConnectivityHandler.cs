using EarableLibrary;
using StepDetectionLibrary;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
		private readonly Input _stepDetection;
		private IEarable _connectedEarable;
		private bool _connecting;

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


		protected ConnectivityHandler()
		{
			_earableManager = new EarableLibrary.EarableLibrary();
			_stepDetection = new Input();
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool EarableConnected => _connectedEarable != null;

		/// <summary>
		/// 
		/// </summary>
		public string EarableName
		{
			get => _connectedEarable?.Name;
		}

		/// <summary>
		/// Tries establishing a BLE connection to a bonded EarableDevice.
		/// </summary>
		/// <returns>null if connection failed, the newly connected device otherwise</returns>
		public virtual async Task<bool> Connect()
		{
			if (EarableConnected || _connecting) return false;

			_connecting = true;
			_connectedEarable = await _earableManager.ConnectEarableAsync();
			_connecting = false;

			if (_connectedEarable == null) return false;

			_connectedEarable.ConnectionLost += async (s, args) =>
			{
				await Disconnect();
			};

			var imu = _connectedEarable?.GetSensor<MotionSensor>();
			if (imu != null)
			{
				imu.SamplingRate = _stepDetection.SamplingRate;
				imu.ValueChanged += _stepDetection.ValueChanged;
				await imu.StartSamplingAsync();
			}

			var button = _connectedEarable?.GetSensor<PushButton>();
			if (button != null)
			{
				button.ValueChanged += (s, args) =>
				{
					bool released = !args.Pressed;
					if (released) SingletonAudioPlayer.TogglePause();
				};
				await button.StartSamplingAsync();
			}

			ConnectionChanged.Invoke(this, null);

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual async Task Disconnect()
		{
			if (!EarableConnected) return;
			await _connectedEarable.DisconnectAsync();
			_connectedEarable = null;
			ConnectionChanged.Invoke(this, null);
		}

		/// <summary>
		/// Set a new Device name.
		/// </summary>
		/// <param name="name">The new Name.</param>
		public async Task SetDeviceNameAsync(string name)
		{
			await _connectedEarable?.SetNameAsync(name);
		}

		/// <summary>
		/// If bluetooth is turned of while app is sleeping, this will update the app
		/// </summary>
		public async void RefreshAfterSleep()
		{
			if (Plugin.BLE.CrossBluetoothLE.Current.IsOn) { Debug.WriteLine("STILL CONNECTED"); }
			else { await Disconnect(); }
		}

	}
}
