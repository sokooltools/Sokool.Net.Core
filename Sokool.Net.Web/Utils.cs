using System.IO;

namespace Sokool.Net.Web
{
	public static class Utils
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Combines two paths returning a windows formatted physical path.
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public static string CombinePaths(string path1, string path2)
		{
			return Path.Combine(path1, path2.Replace('/', '\\').Trim('~', '\\'));
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Combines three paths returning a windows formatted physical path.
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <param name="path3">The path3.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public static string CombinePaths(string path1, string path2, string path3)
		{
			return Path.Combine(path1, path2.Replace('/', '\\').Trim('~', '\\'), path3.Replace('/', '\\').Trim('~','\\'));
		}

	}
}
