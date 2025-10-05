namespace WebAPI.Filters.SwaggerFilters
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SwaggerHeaderAttribute : Attribute
	{
		public string Name { get; }
		public string Description { get; }
		public bool Required { get; }

		public SwaggerHeaderAttribute(string name, string description, bool required = false)
		{
			Name = name;
			Description = description;
			Required = required;
		}
	}
}
