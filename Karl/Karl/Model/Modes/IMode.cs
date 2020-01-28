namespace Karl.Model
{
	/// <summary>
	/// A general Mode.
	/// </summary>
	public interface IMode
	{
		/// <summary>
		/// The name of the mode.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Activate this mode.
		/// </summary>
		void Activate();

		/// <summary>
		/// Deactivate this mode.
		/// </summary>
		void Deactivate();
	}
}
