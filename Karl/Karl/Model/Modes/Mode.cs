namespace Karl.Model
{
	/// <summary>
	/// An abstract Mode.
	/// </summary>
	public abstract class Mode
	{
		private bool _active;

		/// <summary>
		/// The name of this mode.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Whether this mode is currently active or not.
		/// Write access allows (de-)activating this mode.
		/// </summary>
		public bool Active
		{
			get => _active;
			set
			{
				if (_active == value) return;
				if (value) _active = Activate();
				else _active = !Deactivate();
			}
		}

		/// <summary>
		/// Activate this mode.
		/// </summary>
		/// <returns>false if activation is currently impossible, true otherwise</returns>
		protected abstract bool Activate();

		/// <summary>
		/// Deativate this mode.
		/// </summary>
		/// <returns>false if deactivation is currently impossible, true otherwise</returns>
		protected abstract bool Deactivate();
	}
}
