using System;
using System.ComponentModel.DataAnnotations;

namespace Sokool.Net.Web.Utilities
{
	public sealed class ValidEmailDomainAttribute : ValidationAttribute
	{
		private readonly string _allowedDomain;

		public ValidEmailDomainAttribute(string allowedDomain)
		{
			_allowedDomain = allowedDomain;
		}

		public override bool IsValid(object value)
		{
			string[] strings = value.ToString()?.Split('@');
			return strings != null && String.Equals(strings[1], _allowedDomain, StringComparison.CurrentCultureIgnoreCase);
		}
	}
}
