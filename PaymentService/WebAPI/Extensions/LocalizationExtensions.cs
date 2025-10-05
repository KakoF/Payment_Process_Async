using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebAPI.Extensions
{
	public static class LocalizationExtensions
	{
		public static void UseEnglishLocalization(this WebApplication app)
		{
			var defaultCulture = new CultureInfo("en-US");

			CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
			CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

			var localizationOptions = new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(defaultCulture),
				SupportedCultures = new[] { defaultCulture },
				SupportedUICultures = new[] { defaultCulture }
			};

			app.UseRequestLocalization(localizationOptions);
		}
	}

}
