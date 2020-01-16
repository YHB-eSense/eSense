using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Represents a generic sensor.
	/// </summary>
	public interface ISensor
	{
	}

	/// <summary>
	/// Represents a sensor which allows subscription to value updates.
	/// Value refreshs (and therefore notifications) only happen during the sampling process, which must be started manually.
	/// Sampling either runs at a certain rate or can be event-based, depending of the acutal implementation.
	/// </summary>
	/// <typeparam name="T">Type of submitted sensor data</typeparam>
	public interface ISubscribableSensor<T> : ISensor
	{
		/// <summary>
		/// Event which is invoked each time, the value gets refreshed.
		/// </summary>
		event EventHandler<T> ValueChanged;

		/// <summary>
		/// (Targeted) number of value-refreshs per seconds.
		/// -1 indicates that the sensor is event-based.
		/// </summary>
		int SamplingRate { get; set; }

		/// <summary>
		/// Start sampling process.
		/// </summary>
		Task StartSamplingAsync();

		/// <summary>
		/// Stop sampling process.
		/// </summary>
		Task StopSamplingAsync();

	}

	/// <summary>
	/// Represents a sensor which can be read from.
	/// </summary>
	/// <typeparam name="T">Type of returned sensor readings</typeparam>
	public interface IReadableSensor<T> : ISensor
	{
		/// <summary>
		/// Query the sensor for its current value.
		/// </summary>
		/// <returns>Current sensor reading</returns>
		Task<T> ReadAsync();
	}
}
