using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LMS.Helpers
{
	public class LowercasePathOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			// Convert each path in the Swagger operation to lowercase
			if (operation.Parameters == null)
				return;

			foreach (var parameter in operation.Parameters)
			{
				if (parameter.In == ParameterLocation.Path && parameter.Name != null)
				{
					parameter.Name = parameter.Name.ToLowerInvariant();
				}
			}
		}
	}
}
