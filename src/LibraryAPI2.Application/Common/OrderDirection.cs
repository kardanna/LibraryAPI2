using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using LibraryAPI2.Application.Exceptions;

namespace LibraryAPI2.Application.Common;

[TypeConverter(typeof(OrderDirectionConverter))]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderDirection
{
    Asc,
    Desc
}

public class OrderDirectionConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            if (!Enum.TryParse<OrderDirection>(stringValue, true, out var direction))
            {
                throw new BadRequestException($"Invalid OrderDirection value '{stringValue}'. Allowed values: Asc, Desc.");
            }
            return direction;
        }

        return base.ConvertFrom(context, culture, value);
    }
}