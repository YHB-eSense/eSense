using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class AudioLib
	{
		private IAudioLib lib;

		protected void useBasicAudioLib()
		{
			lib = BasicAudioLib.SingletonBasicAudioLib;
		}


	}
	internal interface IAudioLib
	{
	}

	internal interface ITrack
	{
		string Name { get; }
	}
}
