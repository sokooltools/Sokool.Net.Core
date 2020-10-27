using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
		/// The extension used to limit the results. [Default is ".*"  which returns all file types in the physical folder.]
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
				fileInfo => { Add(new Video(vf, fileInfo.FullName)); }
			);
		}
	}

	//public static class VideosViewModelExt
	//{
	//	//------------------------------------------------------------------------------------------------------------------------
	//	/// <summary>
	//	/// Sorts the data in this collection in ascending order by default.
	//	/// </summary>
	//	/// <param name="videos">The cb.</param>
	//	/// <param name="orderbyString">A string containing the property of the video to order by [default is "Name"] and the direction [default is
	//	/// ascending].</param>
	//	/// <returns></returns>
	//	//------------------------------------------------------------------------------------------------------------------------
	//	public static IEnumerable<Video> Sort(this IEnumerable<Video> videos, string orderbyString)
	//	{
	//		//object OrderByFunc(Video m) => m.GetType().GetProperty(orderbyProperty)?.GetValue(m);
	//		if (videos == null)
	//		{
	//			throw  new ArgumentNullException(nameof(videos));
	//		}
	//		string videoProperty = "name";
	//		bool isDescendingOrder = false;
	//		if (!String.IsNullOrEmpty(orderbyString))
	//		{
	//			string[] obs = orderbyString.Split("_", StringSplitOptions.RemoveEmptyEntries);
	//			if (obs.Any())
	//			{
	//				videoProperty = obs[0].ToLower();
	//				if (obs.Length > 1)
	//				{
	//					isDescendingOrder = obs[1].ToLower().StartsWith("desc");
	//				}
	//			}
	//		}
	//		Func<Video, object> orderbyFunc;
	//		if (videoProperty == "length")
	//			orderbyFunc = m => m.Length;
	//		else
	//			orderbyFunc = m => m.Name;
	//		return isDescendingOrder ? videos.OrderByDescending(orderbyFunc) : videos.OrderBy(orderbyFunc);
	//	}
	//}
}
