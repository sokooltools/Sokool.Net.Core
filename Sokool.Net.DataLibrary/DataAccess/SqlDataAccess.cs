using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

//using Dapper;

namespace Sokool.Net.DataLibrary.DataAccess
{
	public static class SqlDataAccess
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a list containing data of type T from the databaase.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql">The SQL statement.</param>
		/// <returns>A list of data of type T</returns>
		//------------------------------------------------------------------------------------------------------------------------
		public static List<T> LoadData<T>(string sql)
		{
			using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
			{
				//return cnn.Query<T>(sql).ToList();
				return new List<T>();
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the data of type T to the database.
		/// </summary>
		/// <typeparam name="T">THe datatype</typeparam>
		/// <param name="sql">The SQL statement.</param>
		/// <param name="data">The data to save.</param>
		/// <returns>The number of rows affected</returns>
		//------------------------------------------------------------------------------------------------------------------------
		public static int SaveData<T>(string sql, T data)
		{
			using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
			{
				//object obj = cnn.ExecuteScalar<T>(sql, data);
				//return (int?)obj ?? 0;
				return 0;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <param name="connectionName">Name of the connection.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		private static string GetConnectionString(string connectionName = "MVCDemoDB")
		{
			//return "Data Source=(localdb)\\ProjectsV13;Integrated Security=True;Connect Timeout=60;"
			//	+ "Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			//return ConfigurationManager<>.ConnectionStrings[connectionName].ConnectionString;
			return "";
		}

	}
}