using Karl.Data;
using Moq;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
using Karl.Model;

namespace UnitTesting.ModelTests
{
	public class BasicLibTest
	{
		public BasicLibTest()
		{
			Tests.Xamarin.Forms.Mocks.MockForms.Init();
		}

		[Fact]
		[TestBeforeAfter]
		public void AddSongTest()
		{


		}

	}


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class TestBeforeAfter : BeforeAfterTestAttribute
	{
		System.Reflection.FieldInfo Instance;

		public override void Before(MethodInfo methodUnderTest)
		{
			Instance = typeof(BasicAudioTrackDatabase).GetField("_singletonDatabase", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			Mock<BasicAudioTrackDatabase> mockSingleton = new Mock<BasicAudioTrackDatabase>();
			Instance.SetValue(null, mockSingleton.Object);
			Debug.WriteLine(methodUnderTest.Name);
		}

		public override void After(MethodInfo methodUnderTest)
		{
			Instance.SetValue(null, null);

			Debug.WriteLine(methodUnderTest.Name);
		}
	}
}
