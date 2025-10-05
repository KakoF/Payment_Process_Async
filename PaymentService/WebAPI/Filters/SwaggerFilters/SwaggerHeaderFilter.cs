using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Filters.SwaggerFilters
{
	public class SwaggerHeaderFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var headerAttributes = context.MethodInfo
				.GetCustomAttributes(typeof(SwaggerHeaderAttribute), false)
				.Cast<SwaggerHeaderAttribute>();

			foreach (var attr in headerAttributes)
			{
				operation.Parameters ??= new List<OpenApiParameter>();

				operation.Parameters.Add(new OpenApiParameter
				{
					Name = attr.Name,
					In = ParameterLocation.Header,
					Required = attr.Required,
					Description = attr.Description,
					Schema = new OpenApiSchema { Type = "string" }
				});
			}
		}
	}

}
