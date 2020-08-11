namespace Sokool.Net.DataLibrary.Data
{
	public class Photo
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the virtual folder path.
		/// </summary>
		//------------------------------------------------------------------------------------------------------------------------
		public string Path { get; private set; }

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the description.
		/// </summary>
		//------------------------------------------------------------------------------------------------------------------------
		public string Description { get; private set; }

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Photo" /> class.
		/// </summary>
		/// <param name="virtualFolder">The virtual folder.</param>
		/// <param name="description">The description.</param>
		//------------------------------------------------------------------------------------------------------------------------
		public Photo(string virtualFolder, string description)
		{
			Path = virtualFolder;
			Description = description;
		}
	}
}