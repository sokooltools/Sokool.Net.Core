using System;
using System.IO;

namespace Sokool.Net.DataLibrary.Data
{
	public class Video
	{
		public string VirtualFolder { get; private set; }

		public string Name { get; private set; }

		public string FileName { get; private set; }

		public string FileType { get; private set; }

		public long Length { get; private set; }

		public string Lyrics { get; private set; }

		//public DateTime CreationTime { get; private set; }

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Video" /> class.
		/// </summary>
		/// <param name="virtualFolder">The virtual folder path.</param>
		/// <param name="physicalFolder">The physical folder path.</param>
		//------------------------------------------------------------------------------------------------------------------------
		public Video(string virtualFolder, string physicalFolder)
		{
			VirtualFolder = virtualFolder.TrimStart('~');
			FileName = Path.GetFileName(physicalFolder) ?? String.Empty;
			Name = Path.GetFileNameWithoutExtension(FileName);
			FileType = GetMimeTypeFromExtension(Path.GetExtension(FileName));
			var fi = new FileInfo(physicalFolder);
			Length = fi.Length;
			string txtFile = Path.ChangeExtension(physicalFolder, ".txt");
			Lyrics = File.Exists(txtFile) ? File.ReadAllText(txtFile) : String.Empty;
			//CreationTime = fi.CreationTime;
		}

		private static string GetMimeTypeFromExtension(string extension) // TODO: Use System.Web.MimeMapping.GetMimeMapping   -- MimeMapping.GetMimeMapping(fileName);
		{
			switch (extension)
			{
				case ".flv":
					return "video/x-flv";
				case ".dv":
					return "video/x-dv";
				case ".m2v":
					return "video/mpeg";
				default:
					return "video/mp4";
			}
		}
	}
}