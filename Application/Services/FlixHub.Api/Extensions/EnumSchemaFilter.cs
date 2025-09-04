namespace FlixHub.Api.Extensions;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var underlyingType = Enum.GetUnderlyingType(context.Type);

            // Set the schema type to match the underlying enum type
            schema.Type = "integer";
            // Clear existing enum values
            schema.Enum.Clear();

            // Add enum values dynamically based on the underlying enum type (byte, short, etc.)
            var enumNames = Enum.GetNames(context.Type);
            var enumValues = Enum
                .GetValues(context.Type).Cast<object>()
                .Select(e => Convert.ChangeType(e, underlyingType));

            // Add the appropriate integer values to the schema
            foreach (var value in enumValues)
            {
                // Handle different underlying enum types (byte, short, int, long, etc.)
                if (underlyingType == typeof(byte))
                {
                    schema.Enum.Add(new OpenApiInteger((byte)value));
                }
                else if (underlyingType == typeof(short))
                {
                    schema.Enum.Add(new OpenApiInteger((short)value));
                }
                else if (underlyingType == typeof(int))
                {
                    schema.Enum.Add(new OpenApiInteger((int)value));
                }
                else if (underlyingType == typeof(long))
                {
                    schema.Enum.Add(new OpenApiInteger(Convert.ToInt32(value)));  // Convert long to int for Swagger UI
                }
                else
                {
                    throw new InvalidOperationException($"Unhandled enum underlying type: {underlyingType}");
                }
            }

            // Add x-enumNames extension for enum names in Swagger
            var enumNamesArray = new OpenApiArray();
            foreach (var name in enumNames)
                enumNamesArray.Add(new OpenApiString(name));

            schema.Extensions.Add("x-enumNames", enumNamesArray);

            // Add description with name = value mapping
            schema.Description = string.Join("\n", enumNames.Zip(enumValues, (name, value) => $"{name} = {value}"));
        }
    }
}