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
		/// Whether this mode is currently activated or not
		/// </summary>
		bool Activated { get; set; }
	}
}
