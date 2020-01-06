namespace EarableLibrary
{
	public interface IAudioStream
	{
		bool Open();
		
		bool Close();
		
		void Write(byte[] data);
	}
}
