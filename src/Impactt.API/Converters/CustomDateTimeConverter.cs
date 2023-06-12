using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Impactt.API.Exceptions;

namespace Impactt.API.Converters;

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateTime.TryParseExact(reader.GetString(), DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
        {
            return dateTime;
        }

        throw new ApiException("sana noto'g'ri kiritilgan (dd-MM-yyyy HH:mm:ss)", 400);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateTimeFormat));
    }
}