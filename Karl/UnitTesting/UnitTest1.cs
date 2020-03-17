using Karl;
using Karl.ViewModel;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace UnitTesting
{
	public class UnitTest1
	{
		public UnitTest1()
		{
			Tests.Xamarin.Forms.Mocks.MockForms.Init();
		}

		[Fact]
		public void ApplicationIsNotNull()
		{	
			var app = new App();
			Assert.NotNull(app);
		}

		[Fact]
		[TestBeforeAfter]
		public void testSettings()
		{
			SettingsPageVM spvw = new SettingsPageVM();
			spvw.ResetStepsCommand.Execute(null);
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
