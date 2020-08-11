using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web.Models
{
	public class VideoModel
	{
		//public string VirtualFile { get; private set; }
		
		//public string Name { get; private set; }

		//public string FileName { get; private set; }

		//public string FileType { get; private set; }

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="VideoModel" /> class.
		/// </summary>
		/// <param name="virtualFolder">The virtual folder.</param>
		/// <param name="fileName">Name of the file.</param>
		//------------------------------------------------------------------------------------------------------------------------
		public VideoModel(string virtualFolder, string fileName)
		{
			//VirtualFile = virtualFolder + fileName;
			//FileName = fileName;
			//Name = Path.GetFileNameWithoutExtension(fileName);
			//FileType = "video/mp4";

			_ = new Video(virtualFolder, fileName);
		}
	}
}