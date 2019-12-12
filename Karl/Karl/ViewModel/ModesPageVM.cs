using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Karl.ViewModel
{
	public class ModesPageVM : INotifyPropertyChanged
	{
		private AppLogic AppLogic;
		public ICommand ActivateModeCommand;
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
			ActivateModeCommand = new Command<Mode>(ActivateMode);
			Modes = new ObservableCollection<Mode>();
		}

		public void ActivateMode(Mode mode)
		{
			//AppLogic
		}
		
		public void GetModes()
		{
			Modes = new ObservableCollection<Mode>(AppLogic.ModeHandler.Modes);
			foreach (var data in Modes) Debug.WriteLine(data.Name);
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
