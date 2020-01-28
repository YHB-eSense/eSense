using EarableLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StepDetectionLibrary
{

	/// <summary>
	/// struct with accerleration and gyro data for all 3 axes
	/// </summary>
	public class AccGyroData
	{
		public readonly AccData AccData;
		public readonly GyroData GyroData;
		public readonly int DataLength;
		public int SamplingRate;
		public double LengthInSeconds => (double) DataLength / SamplingRate;

		public AccGyroData(int dataLength, int samplingRate)
		{
			DataLength = dataLength;
			SamplingRate = samplingRate;
			AccData = new AccData(dataLength);
			GyroData = new GyroData(dataLength);
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
	/// Takes motion-samples as input and sends them to <see cref="StepDetectionAlg"/> in chunks of fixed size.
	/// </summary>
	public class Input : IObservable<AccGyroData>
	{


		private List<IObserver<AccGyroData>> _observers;
		private AccGyroData _chunk;
		private int _counter;
		public Input()
		{
			_observers = new List<IObserver<AccGyroData>>();
			_counter = 0;
			_chunk = new AccGyroData(DataLength, SamplingRate);
			Subscribe(new StepDetectionAlg());
		}

		/// <summary>
		/// Amount of samples in one batch.
		/// </summary>
		public int DataLength { get => 50; } // TODO: make this configurable

		public int SamplingRate { get => 50; } // TODO: make this configurable

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
			// TODO: use args.SampleId to check for missed samples
			_chunk.AccData.Xacc[_counter] = args.Acc.x;
			_chunk.AccData.Yacc[_counter] = args.Acc.y;
			_chunk.AccData.Zacc[_counter] = args.Acc.z;
			_chunk.GyroData.Xgyro[_counter] = args.Gyro.x;
			_chunk.GyroData.Ygyro[_counter] = args.Gyro.y;
			_chunk.GyroData.Zgyro[_counter] = args.Gyro.z;
			_counter++;
			if (_counter == DataLength)
			{
				Update(_chunk);
				// batch = new AccGyroData(DataLength, SamplingRate);
				_counter = 0;
			}
		}
	}
}
