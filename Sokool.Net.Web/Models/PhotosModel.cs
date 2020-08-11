using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web.Models
{
	public class PhotosModel : ConcurrentBag<Photo>
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="PhotosModel" /> class.
		/// </summary>
		/// <param name="virtualFolder">The virtual folder containing the images.</param>
		/// <param name="physicalFolder">The physical folder containing the images.</param>
		/// <param name="extension">The extension used to limit the results. [All files in the virtual folder are returned by default]</param>
		//------------------------------------------------------------------------------------------------------------------------
		public PhotosModel(string virtualFolder, string physicalFolder, string extension = ".*")
		{
			IEnumerable<FileInfo> fileSystemEntries =
				new DirectoryInfo(physicalFolder).EnumerateFiles("*" + extension, SearchOption.TopDirectoryOnly);

			var options = new ParallelOptions
			{
				MaxDegreeOfParallelism = Environment.ProcessorCount * 2,
			};
			Parallel.ForEach(fileSystemEntries, options,
				(fileInfo) =>
				{
					Add(new Photo(String.Concat(virtualFolder, fileInfo.Name), Path.GetFileNameWithoutExtension(fileInfo.Name)));
				}
			);
		}
	}
}