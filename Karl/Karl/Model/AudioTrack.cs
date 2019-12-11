using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public partial class AudioTrack
	{
		private IAudioTrack audioTrack;
		public double Duration
		{
			get
			{
				//todo
				return 0;
			}
		}
		public Image Cover
		{
			get
			{
				//todo
				return null;
			}
		}
		public double CurrentPosition
		{
			get
			{
				//todo
				return 0;
			}
		}

		private interface IAudioTrack
		{
			double Duration { get; }
			Image Cover { get; }
			double CurrentPosition { get; }
			//todo
		}
	}
}
