using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public class AudioTrack
	{
		public double Duration { get; set; }
		public Image Cover { get; set; }
		public double CurrentPosition { get; set; }
	}

	internal interface IAudioTrack
	{

	}
}
