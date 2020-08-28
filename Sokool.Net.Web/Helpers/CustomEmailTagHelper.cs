using Microsoft.AspNetCore.Razor.TagHelpers;

// Make sure to add the following to the _ViewImports.cshtml
//	@addTagHelper *, Sokool.Net.Web

//
// This will change the following:
//		<custom-email my-email="support@sokool.net"></custom-email>
// into this:
//		<a href="mailto:support@sokool.net" id="my-email-id" name="my-email-name">support@sokool.net</a>
//
namespace Sokool.Net.Web.Helpers
{
	public class CustomEmailTagHelper : TagHelper
	{
		public string MyEmail { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "a";
			output.Attributes.SetAttribute("href", $"mailto:{MyEmail}");
			output.Attributes.Add("id","my-email-id");
			output.Attributes.Add("name","my-email-name");
			output.Content.SetContent(MyEmail);
		}
	}
}
