#define TESTING
using Karl.Model;
using Karl.View;
using Karl.ViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class MainPageVMTests
	{
		[Fact]
		public void AudioPlayerPageCommandTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			//test
			vm.AudioPlayerPageCommand.Execute(null);
			Assert.Equal(typeof(AudioPlayerPage), vm.NavHandler.CurrentPageType);
		}

		[Fact]
		public void AudioLibPageCommandTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			//test
			vm.AudioLibPageCommand.Execute(null);
			Assert.Equal(typeof(AudioLibPage), vm.NavHandler.CurrentPageType);
		}

		[Fact]
		public void TryConnectCommandTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			int i = 0;
			vm.PropertyChanged += (sender, e) => i++;
			//test
			vm.TryConnectCommand.Execute(null);
			Assert.Equal(1, i);
		}

		[Fact]
		public void ModesPageCommandTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			//test
			vm.ModesPageCommand.Execute(null);
			Assert.Equal(typeof(ModesPage), vm.NavHandler.CurrentPageType);
		}

		[Fact]
		public void SettingsPageCommandTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			//test
			vm.SettingsPageCommand.Execute(null);
			Assert.Equal(typeof(SettingsPage), vm.NavHandler.CurrentPageType);
		}

		[Fact]
		public void HelpCommandTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			vm.HelpVisible = false;
			//test
			vm.HelpCommand.Execute(null);
			Assert.True(vm.HelpVisible);
		}

		[Fact]
		public void RefreshTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			int i = 0;
			vm.PropertyChanged += (sender, e) => i++;
			//test
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			Assert.Equal(2, i);
			i = 0;
			//test
			SettingsHandler.SingletonSettingsHandler.DeviceName = "test";
			Assert.Equal(1, i);
			i = 0;
			//test
			SettingsHandler.SingletonSettingsHandler.ResetSteps();
			Assert.Equal(2, i);
			/*
			i = 0;
			await vm.ConHandler.Disconnect();
			Assert.Equal(3, i);
			TODO
			*/
		}

		[Fact]
		public void PropertyTest()
		{
			//setup
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new MainPageVM_NEW();
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			//test
			Assert.Equal("Device: ", vm.DeviceName);
			Assert.Equal("Steps: 0", vm.StepsAmount);
			Assert.Equal(vm.IconOn, vm.Icon);
		}

		internal class MainPageVM_NEW : MainPageVM
		{
			protected override void InitializeSingletons()
			{
				_navHandler = new NavigationHandler_NEW();
				_connectivityHandler = new ConnectivityHandler_NEW();
			}
			protected override void NavigateWrapper() { }
			public NavigationHandler NavHandler { get => _navHandler; }
			public ConnectivityHandler ConHandler { get => _connectivityHandler; }
			public ImageSource IconOn { get => _iconOn; }
		}

		internal class NavigationHandler_NEW : NavigationHandler
		{
			public override Type CurrentPageType { get; set; }
			public override async void GotoPage<T>() { CurrentPageType = typeof(T); }
		}

		internal class ConnectivityHandler_NEW : ConnectivityHandler
		{
			public event ConnectionEventHandler ConnectionChanged;
			public override async Task<bool> Connect() { return true; }
			public override async Task Disconnect() { ConnectionChanged?.Invoke(this, null); }
			public override bool EarableConnected { get => true; }
		}

	}
}
