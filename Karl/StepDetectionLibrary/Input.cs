using EarableLibrary;
using System;
using System.Collections.Generic;

namespace StepDetectionLibrary
{

	/// <summary>
	/// class with accerleration and gyro data for all 3 axes
	/// </summary>
	public class AccGyroData
	{
		/// <summary>
		/// acceleration data of 3 axis
		/// </summary>
		public readonly AccData AccData;

		/// <summary>
		/// gyroscope data of 3 axis
		/// </summary>
		public readonly GyroData GyroData;

		/// <summary>
		/// datalength
		/// </summary>
		public readonly int DataLength;

		/// <summary>
		/// samplingrate
		/// </summary>
		public int SamplingRate;

		/// <summary>
		/// length in seconds
		/// </summary>
		public double LengthInSeconds => (double) DataLength / SamplingRate;

		/// <summary>
		/// constructor for accgyrodata
		/// </summary>
		/// <param name="dataLength"> datalength of data</param>
		/// <param name="samplingRate">samplingrate</param>
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
		private byte _lastId = 255;

		/// <summary>
		/// Amount of samples in one batch.
		/// </summary>
		public int DataLength { get => 1; } // TODO: make this configurable

		/// <summary>
		/// SamplingRate
		/// </summary>
		public int SamplingRate { get => 25; } // TODO: make this configurable

		/// <summary>
		/// contructor for input
		/// </summary>
		public Input()
		{
			_observers = new List<IObserver<AccGyroData>>();
			_counter = 0;
			_chunk = new AccGyroData(DataLength, SamplingRate);
			Subscribe(new StepDetectionAlg());
		}
		

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

			/// <summary>
			/// constructor for unsubscriber
			/// </summary>
			/// <param name="observers">list of observers</param>
			/// <param name="observer">observer</param>
			public Unsubscriber(List<IObserver<AccGyroData>> observers, IObserver<AccGyroData> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			/// <summary>
			/// dispose
			/// </summary>
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
			int lost = args.SampleId - _lastId - 1;
			if (lost < 0) lost += 256;
			int lastValid = _counter;
			// if (lost > 0) Debug.WriteLine("Lost {0} samples!", args: lost)
			while (lost > 0)
			{
				// TODO: Interpolate from known values
				_chunk.AccData.Xacc[_counter] = _chunk.AccData.Xacc[lastValid];
				_chunk.AccData.Yacc[_counter] = _chunk.AccData.Yacc[lastValid];
				_chunk.AccData.Zacc[_counter] = _chunk.AccData.Zacc[lastValid];
				_chunk.GyroData.Xgyro[_counter] = _chunk.GyroData.Xgyro[lastValid];
				_chunk.GyroData.Ygyro[_counter] = _chunk.GyroData.Xgyro[lastValid];
				_chunk.GyroData.Zgyro[_counter] = _chunk.GyroData.Xgyro[lastValid];
				_counter++;
				if (_counter == DataLength)
				{
					Update(_chunk);
					_counter = 0;
				}
				lost--;
			}
			_lastId = args.SampleId;
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
				_counter = 0;
			}
		}
	}
}
