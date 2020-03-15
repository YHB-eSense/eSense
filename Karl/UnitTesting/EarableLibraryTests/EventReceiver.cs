using System.Threading.Tasks;

namespace UnitTesting.EarableLibraryTests
{
	public class EventReceiver<T>
	{
		private TaskCompletionSource<T> ReceiverTaskSource;

		public void OnEvent(object sender, T eventArgs)
		{
			ReceiverTaskSource?.SetResult(eventArgs);
		}

		public Task<T> ReceiveOne()
		{
			ReceiverTaskSource = new TaskCompletionSource<T>();
			return ReceiverTaskSource.Task;
		}
	}
}
