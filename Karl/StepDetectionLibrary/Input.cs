using EarableLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StepDetectionLibrary
{

	/// <summary>
	/// struct with accerleration and gyro data for all 3 axes
	/// </summary>
	public struct AccGyroData
	{
		public AccData AccData;
		public GyroData GyroData;
		public const int DATALENGTH = 40;

		public AccGyroData(int a)
		{
			AccData = new AccData(a);
			GyroData = new GyroData(a);
		}

		public AccGyroData(AccData accData, GyroData gyroData)
		{
			AccData = accData;
			GyroData = gyroData;
		}
	}

	/// <summary>
	/// struct with acceleration data for 3 axes
	/// </summary>
	public struct AccData
	{
		public double[] Xacc;
		public double[] Yacc;
		public double[] Zacc;

		public AccData(int a)
		{
			Xacc = new double[a];
			Yacc = new double[a];
			Zacc = new double[a];
		}
		public AccData(double[] xacc, double[] yacc, double[] zacc)
		{
			Xacc = xacc;
			Yacc = yacc;
			Zacc = zacc;
		}
	}

	/// <summary>
	/// struct with gyroscope data for 3 axes
	/// </summary>
	public struct GyroData
	{
		public double[] Xgyro;
		public double[] Ygyro;
		public double[] Zgyro;

		public GyroData(int a)
		{
			Xgyro = new double[a];
			Ygyro = new double[a];
			Zgyro = new double[a];
		}
		public GyroData(double[] xgyro, double[] ygyro, double[] zgyro)
		{
			Xgyro = xgyro;
			Ygyro = ygyro;
			Zgyro = zgyro;
		}
	}

	/// <summary>
	/// gets data from earables and sends them to stepdetectionalg class
	/// </summary>
	public class Input : IObservable<AccGyroData>

	{
		AccGyroData accgyrodata;
		int counter;
		public Input()
		{
			_observers = new List<IObserver<AccGyroData>>();
			counter = 0;
			accgyrodata = new AccGyroData(AccGyroData.DATALENGTH);
			Subscribe(new StepDetectionAlg());
		}

		private List<IObserver<AccGyroData>> _observers;

		public int PreferredSamplingRate { get => 50; }

		/// <summary>
		/// method for subscribing to input
		/// </summary>
		/// <param name="observer">object that wants to observe input</param>
		/// <returns>disposable for unsubscribing</returns>
		public IDisposable Subscribe(IObserver<AccGyroData> observer)
		{
			{
				if (!_observers.Contains(observer))
					_observers.Add(observer);
				return new Unsubscriber(_observers, observer);
			}
		}

		/// <summary>
		/// Unsubscriber
		/// </summary>
		private class Unsubscriber : IDisposable
		{
			private List<IObserver<AccGyroData>> _observers;
			private IObserver<AccGyroData> _observer;

			public Unsubscriber(List<IObserver<AccGyroData>> observers, IObserver<AccGyroData> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}

		/// <summary>
		/// method to update _observer
		/// </summary>
		/// <param name="data">new accleration + gyro data</param>
		public void Update(AccGyroData data)
		{
			foreach (var observer in _observers)
			{
				observer.OnNext(data);
			}
		}

		/// <summary>
		/// method to get data from sensors
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="args">parameter</param>
		public void ValueChanged(object sender, MotionSensorSample args)
		{
			accgyrodata.AccData.Xacc[counter] = args.Acc.x;
			accgyrodata.AccData.Yacc[counter] = args.Acc.y;
			accgyrodata.AccData.Zacc[counter] = args.Acc.z;
			accgyrodata.GyroData.Xgyro[counter] = args.Gyro.x;
			accgyrodata.GyroData.Ygyro[counter] = args.Gyro.y;
			accgyrodata.GyroData.Zgyro[counter] = args.Gyro.z;
			counter++;
			if (counter == AccGyroData.DATALENGTH)
			{
				Update(accgyrodata);
				accgyrodata = new AccGyroData(AccGyroData.DATALENGTH);
				counter = 0;
			}
		}
	}
}
