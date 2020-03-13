using EarableLibrary;
using Moq;
using SQLite;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.StepDetectionLibraryTests
{
	/// <summary>
	/// Class for Modul tests for Stepdetectionlibrary
	/// </summary>
	 public class StepDetectionModulTest
	{
		/// <summary>
		/// Tests if subscriber to output receive if valuechanges
		/// </summary>
		[Fact]
		public void InandOutTests()
		{
			Input TestInput = new Input();
			Mock<IObserver<Output>> MockObserver = new Mock<IObserver<Output>>();
			OutputManager.SingletonOutputManager.Subscribe(MockObserver.Object);
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(1, 2, 3), 1));
			MockObserver.Verify(foo => foo.OnNext(It.IsAny<Output>()));

			FieldInfo field = typeof(OutputManager).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance);
			object Oservers = field.GetValue(OutputManager.SingletonOutputManager);
			List<IObserver<Output>> Observers = (List<IObserver<Output>>)Oservers;
			Observers.Clear();
			OutputManager.SingletonOutputManager.Log.Reset();
		}

		/// <summary>
		/// tests if no step doesnt get detected
		/// </summary>
		[Fact]
		public void NoStepTest()
		{
			Input TestInput = new Input();
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(10, 20, 30), 1));
			Assert.Equal(0, new Output().StepCount());
			Assert.Equal(0, new Output().Frequency());

			OutputManager.SingletonOutputManager.Log.Reset();

		}

		/// <summary>
		/// tests if one step gets detected
		/// </summary>
		[Fact]
		public void StepTest()
		{
			Input TestInput = new Input();
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(1390, 3220, 8830), 1));
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(1390, 220, 830), 1));
			Assert.Equal(1, new Output().StepCount());
			OutputManager.SingletonOutputManager.Log.Reset();
		}

		/// <summary>
		/// tests with a real sample if correct amount of steps get detected
		/// </summary>
		[Fact]
		public void SampleTest()
		{
			string FileName = "18schritte50hz.db";
			string path = (Path.Combine(Environment.CurrentDirectory, @"Data\", FileName));
			TestActivityLog TestActivitylog = new TestActivityLog(path);
			var activityFrames = TestActivitylog.GetData();

			Input TestInput = new Input();

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionSensorSample arg = activityFrames[i].ToMotionSensorSample();
				TestInput.ValueChanged(null, arg);
			}
			Assert.Equal(18, new Output().StepCount());
			OutputManager.SingletonOutputManager.Log.Reset();
		}


		public class TestActivityLog
		{

			private readonly SQLiteConnection _conn;

			public TestActivityLog(string dbPath)
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
}
