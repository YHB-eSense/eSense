using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Karl.ViewModel
{
	public class ModesPageVM : INotifyPropertyChanged
	{
		private AppLogic AppLogic;
		public ICommand MotivationModeCommand;
		public ICommand AutostopModeCommand;
		private ObservableCollection<Mode> modes;

		public ObservableCollection<Mode> Modes
		{
			get
			{
				return modes;
			}
			set
			{
				modes = value;
				OnPropertyChanged("Modes");
			}
		}


		public ModesPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			MotivationModeCommand = new Command<bool>(MotivationMode);
			AutostopModeCommand = new Command<bool>(AutostopMode);
		}

		public void MotivationMode(bool value)
		{
			//AppLogic
		}


		public void AutostopMode(bool value)
		{
			//AppLogic
		}

		private void GetModes()
		{
			//AppLogic
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
