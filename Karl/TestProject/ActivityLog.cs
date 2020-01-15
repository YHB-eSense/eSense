using EarableLibrary;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
	public class ActivityLog
	{
		private readonly ActivityFrame[] buffer;
		private SQLiteAsyncConnection database;
		private int counter;

		private byte lastId;

		public ActivityLog()
		{
			buffer = new ActivityFrame[256];
			counter = 0;
			lastId = 0;
		}

		public async Task<ActivityFrame[]> GetData()
		{
			if (database == null)
			{
				var name = string.Format("activity-log-{0}.db", DateTime.Now.Ticks);
				var path = Path.Combine("C:\\Users\\Leo\\Documents\\PSE\\Daten", name);
				database = new SQLiteAsyncConnection(path);
				ActivityFrame[] data = await database.Table<ActivityFrame>().ToArrayAsync();
				return data;
			}
			return null;

		}



		public void StopLogging()
		{
			if (database != null)
			{
				if (counter != 0)
				{
					ActivityFrame[] notInsertedYet = new ActivityFrame[counter];
					Array.Copy(buffer, 0, notInsertedYet, 0, counter);
					database.InsertAllAsync(notInsertedYet).Wait();
					counter = 0;
				}
				database.CloseAsync();
				database = null;
			}
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
		public MotionArgs ToMotionArgs()
		{
			TripleShort acc = new TripleShort(AccX, AccY, AccZ);
			TripleShort gyro = new TripleShort(GyroX, GyroY, GyroZ);

			return new MotionArgs(gyro, acc, Counter);
		
		}

	}


}
