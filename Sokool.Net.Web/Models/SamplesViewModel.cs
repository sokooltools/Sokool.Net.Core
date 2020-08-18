using System.Collections.Generic;
using System.IO;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web.Models
{
	public class SamplesViewModel : List<Sample>
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SamplesViewModel" /> class.
		/// A list of Sample objects is created from all sample files in the specified virtual folder having the specified
		/// extension.
		/// </summary>
		/// <param name="virtualFolder">The virtual folder.</param>
		/// <param name="physicalPath">The physical path.</param>
		/// <param name="extension">The extension. [All files in the virtual folder are returned by default]</param>
		//------------------------------------------------------------------------------------------------------------------------
		public SamplesViewModel(string virtualFolder, string physicalPath, string extension = ".*")
		{
			var di = new DirectoryInfo(physicalPath);
			foreach (FileInfo file in di.EnumerateFiles("*" + extension, SearchOption.TopDirectoryOnly))
			{
				Add(new Sample($"/{virtualFolder}/{file.Name}", file.FullName));
			}
		}
	}
}