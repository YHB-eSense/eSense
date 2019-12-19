using System;

namespace StepDetectionLibrary
{

	/// <summary>
	/// struct with accerleration and gyro data for all 3 axes
	/// </summary>
	public struct AccGyroData
	{
		public short[] xacc;
		public short[] yacc;
		public short[] zacc;
		public short[] xgyro;
		public short[] ygyro;
		public short[] zgyro;
	}
	/// <summary>
	/// gets data from earables and sends them to stepdetectionalg class
	/// </summary>
	public class Input : IObservable<AccGyroData>

	{
		private List<IObserver<Output>> observers;
		/// <summary>
		/// method for subscribing to input
		/// </summary>
		/// <param name="observer">object that wants to observe input</param>
		/// <returns>disposable for unsubscribing</returns>
		public IDisposable Subscribe(IObserver<AccGyroData> observer)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// method to update observers
		/// </summary>
		/// <param name="data">new accleration + gyro data</param>
		public void Update(AccGyroData data)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// method to get data from sensors
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="args">parameter</param>
		public void ValueChanged(object sender, EventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
