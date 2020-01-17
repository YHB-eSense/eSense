using EarableLibrary;
using SQLite;
using System;

namespace TestProject
{
	public class ActivityLog
	{
		private readonly SQLiteConnection _conn;

		public ActivityLog(string dbPath)
		{
			_conn = new SQLiteConnection(dbPath);
		}

		public ActivityFrame[] GetData()
		{
			var table = _conn.Table<ActivityFrame>();
			return table.ToArray();
		}
	}

	[Serializable]
	public class ActivityFrame
	{

		public ActivityFrame() { }
		[PrimaryKey, AutoIncrement]
		public long Id { get; set; }
		public short AccX { get; set; }
		public short AccY { get; set; }
		public short AccZ { get; set; }
		public short GyroX { get; set; }
		public short GyroY { get; set; }
		public short GyroZ { get; set; }
		public byte Counter { get; set; }
		public MotionSensorSample ToMotionSensorSample()
		{
			TripleShort acc = new TripleShort(AccX, AccY, AccZ);
			TripleShort gyro = new TripleShort(GyroX, GyroY, GyroZ);

			return new MotionSensorSample(gyro, acc, Counter);
		
		}

	}


}
