namespace Sokool.Net.Web.Utilities
{
	public static class ExceptionHandler
	{
		public static (int statusCode, string Desc1, string Desc2) GetDesc(int statusCode)
		{
			return statusCode switch
			{
				400 => (400, "BadRequest", "A bad request was made"),
				401 => (401, "Unauthorized", "Access is denied due to invalid credentials"),
				403 => (403, "Forbidden: Access is denied", "Access to the requested resource is forbidden"),
				404 => (404, "File or directory not found", "The resource you requested may have been removed, had its name changed, or is temporarily unavailable."),
				405 => (405, "HTTP verb used to access this page not allowed", "The page you are looking for cannot be displayed because an invalid method (HTTP verb) was used to attempt access."),
				406 => (406, "Client browser does not accept the MIME type of the requested page", "The page you are looking for cannot be opened by your browser because it has a file name extension that your browser does not accept."),
				408 => (408, "RequestTimeout", "The request timed out"),
				500 => (500, "Internal server error", "There is a problem with the resource you are looking for, and it cannot be displayed"),
				503 => (503, "Service Unavailable", "The server cannot handle the request (because it is overloaded or down for maintenance)"),
				_ => (statusCode, "Unknown", "An error occured processing your request")
			};
		}
	}
}