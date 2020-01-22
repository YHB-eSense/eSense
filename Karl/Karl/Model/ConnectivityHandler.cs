using System;
using EarableLibrary;
using System.Diagnostics;
using System.Threading.Tasks;
using StepDetectionLibrary;
using System.Collections.Generic;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the conntection to the Earables Library and Step Detection Library
	/// </summary>
	public class ConnectivityHandler
	{

		private static ConnectivityHandler _connectivityHandler;

		//Eventhandling
		public event EventHandler<ConnectionEventArgs> ConnectionChanged;

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

		private ConnectivityHandler()
		{
			_earableManager = new EarableLibrary.EarableLibrary();
			_stepDetection = new Input();
		}

		public bool EarableConnected
		{
			get
			{
				if (_connectedEarable == null) return false;
				if (!_connectedEarable.IsConnected())
				{
					_connectedEarable = null;
					return false;
				}
				return true;
			}
		}

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

			var imu = _connectedEarable.GetSensor<MotionSensor>();
			imu.SamplingRate = _stepDetection.PreferredSamplingRate;
			imu.ValueChanged += _stepDetection.ValueChanged;
			await imu.StartSamplingAsync();

			var button = _connectedEarable.GetSensor<PushButton>();
			button.ValueChanged += (s, args) =>
			{
				Debug.WriteLine("Button pushed: {0}", args.Pressed);
			};
			await button.StartSamplingAsync();

			ConnectionChanged?.Invoke(this, null);

			return true;
		}

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
		public async void SetDeviceName(string name)
		{
			if (!EarableConnected) return;
			await _connectedEarable.SetNameAsync(name);
		}

	}

	public class ConnectionEventArgs : EventArgs
	{
	}
}
