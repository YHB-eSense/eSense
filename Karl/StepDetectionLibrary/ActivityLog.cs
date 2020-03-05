using SQLite;
using System;
using System.Collections.Generic;

namespace StepDetectionLibrary
{
	public class ActivityLog
	{
		private readonly SQLiteConnection _conn;

		/// <summary>
		/// Construct a new ActivityLog.
		/// </summary>
		/// <param name="conn">Database to store the log in</param>
		public ActivityLog(string dbPath)
		{
			_conn = new SQLiteConnection(dbPath);
			_conn.CreateTable<Step>(); // if not exists
		}

		/// <summary>
		/// Get the amount of steps that took place in a time span defined by its start and end time.
		/// </summary>
		/// <param name="since">Beginning of the time span</param>
		/// <param name="to">Optional end of the time span</param>
		/// <returns>Amount of steps</returns>
		public int CountSteps(DateTime? since = null, DateTime? until = null)
		{
			if (since == null) since = DateTime.MinValue;
			if (until == null) until = DateTime.MaxValue;
			return _conn.ExecuteScalar<int>("SELECT COUNT(Taken) FROM Step WHERE Taken BETWEEN ? AND ?", since, until);
		}

		/// <summary>
		/// Get the amount of steps that took place in a time span which ends now with a given length.
		/// </summary>
		/// <param name="duration">Length of the time span</param>
		/// <returns>Amount of steps</returns>
		public int CountSteps(TimeSpan duration)
		{
			var since = DateTime.UtcNow - duration;
			return _conn.ExecuteScalar<int>("SELECT COUNT(Taken) FROM Step WHERE Taken >= ?", since);
		}

		/// <summary>
		/// Return a list of the most recent <see cref="Step"/>s.
		/// </summary>
		/// <param name="n">Maximum amount of steps to return</param>
		/// <returns>List of steps</returns>
		public List<Step> LastSteps(int n)
		{
			return _conn.Query<Step>("SELECT * FROM Step ORDER BY Taken DESC LIMIT ?", n);
		}

		/// <summary>
		/// Write a new step to the log.
		/// </summary>
		/// <param name="step">New step</param>
		public void Add(Step step)
		{
			_conn.Insert(step);
		}

		/// <summary>
		/// Calculates the current step frequency (in Hz) by taking an average of the recent past.
		/// <param name="duration">How far to look back while calculating the average</param>
		/// </summary>
		public double AverageStepFrequency(TimeSpan duration)
		{
			return CountSteps(since: DateTime.UtcNow - duration) / duration.TotalSeconds;
		}

		public void Reset()
		{
			_conn.DropTable<Step>();
			_conn.CreateTable<Step>();
		}
	}

	/// <summary>
	/// Used to store a single detected step.
	/// </summary>
	[Serializable]
	public class Step
	{
		/// <summary>
		/// Point in time at which the steps peak started
		/// </summary>
		[PrimaryKey]
		public DateTime Taken { get; set; }

		/// <summary>
		/// Length of the measured peak
		/// </summary>
		public TimeSpan Duration { get; set; }

		/// <summary>
		/// Measured intensity of the steps peak
		/// </summary>
		public double Intensity { get; set; }
	}
}
