using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

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

		public override void Before(MethodInfo methodUnderTest)
		{
			Debug.WriteLine(methodUnderTest.Name);
		}

		public override void After(MethodInfo methodUnderTest)
		{
			Debug.WriteLine(methodUnderTest.Name);
		}
	}
}
