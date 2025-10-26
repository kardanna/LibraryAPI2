using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using LibraryAPI2.Application.Exceptions;

namespace LibraryAPI2.Application.DTO.Book;

[TypeConverter(typeof(BookOrderByConverter))]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookOrderBy
{
    Title,
    PublishedYear,
    AuthorName
}

public class BookOrderByConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            if (!Enum.TryParse<BookOrderBy>(stringValue, true, out var orderBy))
            {
                throw new BadRequestException($"Invalid '{nameof(BookOrderBy)}' value '{stringValue}'. Allowed values: Title, PublishedYear, AuthorName.");
            }
            return orderBy;
        }

        return base.ConvertFrom(context, culture, value);
    }
}