using ValidatR.Attributes;

namespace ValidatR.AspNet;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseValidatorMiddleware(this IApplicationBuilder self, params Type[] explicitTypes)
    {
        self.UseMiddleware<ValidationExceptionMiddleware>();

        var types = explicitTypes != null && explicitTypes.Length > 0
            ? explicitTypes.ToList()
            : ScanForTypesHavingValidateAttribute();

        foreach (var type in types)
        {
            self.UseMiddleware(typeof(ValidationMiddleware<>).MakeGenericType(type));
        }

        return self;
    }

    private static IEnumerable<Type> ScanForTypesHavingValidateAttribute()
    {
        var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    from properties in type.GetProperties()
                    where properties.CustomAttributes.Any(x => x.AttributeType.Equals(typeof(ValidateAttribute)))
                    select type;

        return types;
    }
}
