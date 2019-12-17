namespace EarableLibrary
{
	public interface IAudioStream
	{
		bool open();
		
		bool close();
		
		void write(byte[] data);
	}
}
