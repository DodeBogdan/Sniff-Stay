using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace SniffAndStay.API.SchemaFilter
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            var descriptions = Enum.GetNames(context.Type)
                .Select((name, index) => $"{index} = {name}");

            schema.Description = (schema.Description is null ? "" : " ")
                + string.Join(", ", descriptions);
        }
    }
}
