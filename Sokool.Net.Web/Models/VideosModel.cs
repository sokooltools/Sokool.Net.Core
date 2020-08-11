using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web.Models
{
	public class VideosModel : ConcurrentBag<Video>
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="VideosModel" /> class.
		/// </summary>
		/// <param name="vf">The virtual folder containing the videos.</param>
		/// <param name="physicalFolderPath">The physical folder path.</param>
		/// <param name="extension">
		/// The extension used to limit the results. [All files in the virtual folder are returned by default]
		/// </param>
		//------------------------------------------------------------------------------------------------------------------------
		public VideosModel(string vf, string physicalFolderPath, string extension = ".*")
		{
			IEnumerable<FileInfo> fileSystemEntries =
				new DirectoryInfo(physicalFolderPath).EnumerateFiles("*" + extension, SearchOption.TopDirectoryOnly);

			var options = new ParallelOptions
			{
				MaxDegreeOfParallelism = Environment.ProcessorCount * 2
			};
			Parallel.ForEach(fileSystemEntries, options,
				(fileInfo) =>
				{
					Add(new Video(vf, fileInfo.FullName));
				}
			);
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sorts the data in this collection in ascending order by default.
		/// </summary>
		/// <param name="isDescendingOrder">
		/// if set to <c>true</c> returns the data in descending order; otherwise returns the data sorted in ascending order by 
		/// default.
		/// </param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public IOrderedEnumerable<Video> Sort(bool isDescendingOrder = false)
		{
			return isDescendingOrder ? this.ToList().OrderByDescending(m => m.Name) : this.ToList().OrderBy(m => m.Name);
		}
	}
}