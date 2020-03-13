using Karl.Model;
using Karl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class ModePageVMTests
	{
		[Fact]
		public void RefreshTest()
		{
			var vm = new ModesPageVM_NEW();
			int i = 0;
			vm.PropertyChanged += (sender, e) => i++;
			//test
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			Assert.Equal(2, i);
			i = 0;
			//test
			SettingsHandler.SingletonSettingsHandler.CurrentColor = SettingsHandler.SingletonSettingsHandler.Colors[0];
			Assert.Equal(2, i);
			/*
			i = 0;
			//test
			await vm.ConHandler.Disconnect();
			Assert.Equal(3, i);
			//TODO
			*/
		}

		internal class ModesPageVM_NEW : ModesPageVM
		{
			protected override void InitializeSingletons()
			{
				_connectivityHandler = new ConnectivityHandler_NEW();
			}
			public ConnectivityHandler ConHandler { get => _connectivityHandler; }
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
