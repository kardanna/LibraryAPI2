using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using LibraryAPI2.Application.Exceptions;

namespace LibraryAPI2.Application.DTO.Author;

[TypeConverter(typeof(AuthorOrderByConverter))]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AuthorOrderBy
{
    Name,
    DateOfBirth
}

public class AuthorOrderByConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            if (!Enum.TryParse<AuthorOrderBy>(stringValue, true, out var orderBy))
            {
                throw new BadRequestException($"Invalid '{nameof(AuthorOrderBy)}' value '{stringValue}'. Allowed values: Name, DateOfBirth.");
            }
            return orderBy;
        }

        return base.ConvertFrom(context, culture, value);
    }
}