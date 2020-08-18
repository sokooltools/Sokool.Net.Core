using System.Collections.Generic;
using System.Threading.Tasks;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.DataLibrary.BusinessLogic
{
	public static class UserProcessor
	{
		public static int CreateUser(int userId, string firstName, string lastName, string emailAddress)
		{
			//var data = new User()
			//{
			//	UserId = userId,
			//	FirstName = firstName,
			//	LastName = lastName,
			//	EmailAddress = emailAddress
			//};

			//const string sql = @"insert into dbo.User (UserId, FirstName, LastName, EmailAddress)
			//		values(@UserId, @FirstName, @LastName, @EmailAddress);";

			//return SqlDataAccess.SaveData(sql, data); // TODO:
			return 0;
		}

		public static List<User> LoadUsers()
		{
			//const string sql = @"select UserId, FirstName, LastName, EmailAddress from dbo.Users;";

			//return SqlDataAccess.LoadData<User>(sql); // TODO:
			return new List<User>();
		}

		public static bool LoginUser(string emailAddress, string password, bool rememberMe)
		{
			return  true;
		}

	}
}