using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web.Models
{
	public class VideosViewModel : ConcurrentBag<Video>
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="VideosViewModel" /> class.
		/// </summary>
		/// <param name="vf">The virtual folder containing the videos.</param>
		/// <param name="physicalFolderPath">The physical folder path.</param>
		/// <param name="extension">
		/// The extension used to limit the results. [All files in the virtual folder are returned by default]
		/// </param>
		//------------------------------------------------------------------------------------------------------------------------
		public VideosViewModel(string vf, string physicalFolderPath, string extension = ".*")
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
		/// <param name="orderbyString">
		/// A string containing the property of the video to order by [default is "Name"] and the direction [default is 
		/// ascending].
		/// </param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public IEnumerable<Video> Sort(string orderbyString)
		{
			//object OrderByFunc(Video m) => m.GetType().GetProperty(orderbyProperty)?.GetValue(m);

			string videoProperty = "name";
			bool isDescendingOrder = false;
			if (!String.IsNullOrEmpty(orderbyString))
			{
				string[] o = orderbyString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
				if (o.Any())
				{
					videoProperty = o[0].ToLower();
					if (o.Length > 1)
					{
						isDescendingOrder = o[1].ToLower().StartsWith("desc");
					}
				}
			}

			Func<Video, object> orderbyFunc;
			if (videoProperty == "length")
				orderbyFunc = m => m.Length;
			else
				orderbyFunc = m => m.Name;

			return isDescendingOrder ? this.ToList().OrderByDescending(orderbyFunc) : this.ToList().OrderBy(orderbyFunc);
		}
	}
}