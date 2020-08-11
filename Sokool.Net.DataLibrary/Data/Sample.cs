using System.IO;

namespace Sokool.Net.DataLibrary.Data
{
	public class Sample
	{
		public string VirtualPath { get; private set; }

		public string PhysicalPath { get; private set; }

		public string Name => Path.GetFileNameWithoutExtension(PhysicalPath);

		//public string FileName => Path.GetFileName(PhysicalPath);

		//public string Extension => Path.GetExtension(PhysicalPath);

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Sample" /> class.
		/// </summary>
		/// <param name="virtualPath">The virtual path.</param>
		/// <param name="physicalPath">The physical file path.</param>
		//------------------------------------------------------------------------------------------------------------------------
		public Sample(string virtualPath, string physicalPath)
		{
			PhysicalPath = physicalPath;
			VirtualPath = virtualPath;
		}
	}
}