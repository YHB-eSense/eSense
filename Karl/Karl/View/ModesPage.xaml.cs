using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Karl.ViewModel;
using FormsControls.Base;
using SkiaSharp;
using Microcharts;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModesPage : ContentPage, IAnimationPage
	{
		private ModesPageVM _modesPageVM;
		List<Microcharts.Entry> entries = new List<Microcharts.Entry>()
		{
			new Microcharts.Entry(200)
			{
				Color = SKColor.Parse("#FF1493"),
				Label = "Avocado",
				ValueLabel = "200"
			},
			new Microcharts.Entry(300)
			{
				Color = SKColor.Parse("#68B9C0"),
				Label = "Apfelsine",
				ValueLabel = "300"
			},
			new Microcharts.Entry(-100)
			{
				Color = SKColor.Parse("#266489"),
				Label = "Pomelo",
				ValueLabel = "-100"
			}

		};

public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromLeft };

		public ModesPage(ModesPageVM modesPageVM)
		{
			InitializeComponent();
			_modesPageVM = modesPageVM;
			BindingContext = _modesPageVM;
			
			CrazyChart.Chart = new BarChart { Entries = entries };
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
