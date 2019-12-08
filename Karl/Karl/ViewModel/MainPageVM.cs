using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Karl.Model;

namespace Karl.ViewModel
{
	public class MainPageVM : INotifyPropertyChanged
	{
		private AppLogic appLogic;

		public MainPageVM(AppLogic appLogic)
		{
			this.appLogic = appLogic;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private string deviceName;
		private string stepsAmount;

		public string DeviceName
		{
			get
			{
				return deviceName;
			}
			set
			{
				if (deviceName != value)
				{
					deviceName = value;
					OnPropertyChanged("DeviceName");
				}
			}
		}

		public string StepsAmount
		{
			get
			{
				return stepsAmount;
			}
			set
			{
				if (stepsAmount != value)
				{
					stepsAmount = value;
					OnPropertyChanged("StepsAmount");
				}
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			if(PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public AppLogic AppLogic
		{
			get => default;
			set
			{
			}
		}
	}
}
