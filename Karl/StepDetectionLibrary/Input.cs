using EarableLibrary;
using System;
using System.Collections.Generic;

namespace StepDetectionLibrary
{

	/// <summary>
	/// struct with accerleration and gyro data for all 3 axes
	/// </summary>
	public struct AccGyroData
	{
		public AccData AccData;
		public GyroData GyroData;
		public const int DATALENGTH = 250;
	}

	/// <summary>
	/// struct with acceleration data for 3 axes
	/// </summary>
	public struct AccData
	{
		public double[] Xacc;
		public double[] Yacc;
		public double[] Zacc;
	}

	/// <summary>
	/// struct with gyroscope data for 3 axes
	/// </summary>
	public struct GyroData
	{
		public short[] Xgyro;
		public short[] Ygyro;
		public short[] Zgyro;
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
			counter = 0;
			accgyrodata = new AccGyroData();
			Subscribe(new StepDetectionAlg());
		}

		private List<IObserver<AccGyroData>> _observers;
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
		public void ValueChanged(object sender, EventArgs args)
		{
			MotionArgs marg = (MotionArgs)args;
			accgyrodata.AccData.Xacc[counter] = marg.Acc.x;
			accgyrodata.AccData.Yacc[counter] = marg.Acc.y;
			accgyrodata.AccData.Zacc[counter] = marg.Acc.z;
			accgyrodata.GyroData.Xgyro[counter] = marg.Gyro.x;
			accgyrodata.GyroData.Ygyro[counter] = marg.Gyro.y;
			accgyrodata.GyroData.Zgyro[counter] = marg.Gyro.z;
			counter++;
			if (counter == AccGyroData.DATALENGTH)
			{
				Update(accgyrodata);
				accgyrodata = new AccGyroData();
				counter = 0;
			}
		}
	}
}
