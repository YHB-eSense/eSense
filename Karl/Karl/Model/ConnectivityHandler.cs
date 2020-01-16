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
		private IEarable _connectedEarable;

		private ConnectivityHandler()
		{
			_earableManager = new EarableLibrary.EarableLibrary();
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

		/// <summary>
		/// Tries establishing a BLE connection to a bonded EarableDevice.
		/// </summary>
		/// <returns>null if connection failed, the newly connected device otherwise</returns>
		public async Task<bool> Connect()
		{
			_connectedEarable = await _earableManager.ConnectEarableAsync();

			if (_connectedEarable == null) return false;

			var input = new Input();
			var accDataXList = new List<double>();
			var accDataYList = new List<double>();
			var accDataZList = new List<double>();
			var gyroDataXList = new List<short>();
			var gyroDataYList = new List<short>();
			var gyroDataZList = new List<short>();
			var imu = _connectedEarable.GetSensor<MotionSensor>();
			imu.SamplingRate = 10;
			imu.ValueChanged += (s, args) =>
			{
				Debug.WriteLine("(Id,Acc(x,y,z))\t{0}\t{1}\t{2}\t{3}", args.SampleId, args.Acc.x, args.Acc.y, args.Acc.z);

				accDataXList.Add(args.Acc.x);
				accDataYList.Add(args.Acc.y);
				accDataZList.Add(args.Acc.z);
				gyroDataXList.Add(args.Gyro.x);
				gyroDataYList.Add(args.Gyro.y);
				gyroDataZList.Add(args.Gyro.z);
				if (accDataXList.Count == AccGyroData.DATALENGTH)
				{
					var accData = new AccData(accDataXList.ToArray(), accDataYList.ToArray(), accDataZList.ToArray());
					var gyroData = new GyroData(gyroDataXList.ToArray(), gyroDataYList.ToArray(), gyroDataZList.ToArray());
					input.Update(new AccGyroData(accData, gyroData));
				}
			};
			await imu.StartSamplingAsync();

			var button = _connectedEarable.GetSensor<PushButton>();
			button.ValueChanged += (s, args) =>
			{
				Debug.WriteLine("Button pushed: {0}", args.Pressed);
			};
			await button.StartSamplingAsync();

			return true;
		}

		public async Task Disconnect()
		{
			if (!EarableConnected) return;
			await _connectedEarable.DisconnectAsync();
			_connectedEarable = null;
		} 

		/// <summary>
		/// Set a new Device name.
		/// </summary>
		/// <param name="name">The new Name.</param>
		public void SetDeviceName(String name)
		{
			if (!EarableConnected) return;
			_connectedEarable.Name = name;
		}
	}
}
