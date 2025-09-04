namespace Tasheer.Shared.Helper;

public class NumberToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle reading the value based on the actual type
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt64().ToString();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }
        else
        {
            throw new JsonException("Unexpected token type");
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        // Handle writing the value
        writer.WriteStringValue(value);
    }
}

public class StringToInt32Converter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle reading the value based on the actual type
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return Convert.ToInt32(reader.GetString());
        }
        else
        {
            throw new JsonException("Unexpected token type");
        }
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        // Handle writing the value
        writer.WriteStringValue(value.ToString());
    }
}

public class StringToInt64Converter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle reading the value based on the actual type
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt64();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return Convert.ToInt64(reader.GetString());
        }
        else
        {
            throw new JsonException("Unexpected token type");
        }
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        // Handle writing the value
        writer.WriteStringValue(value.ToString());
    }
}

public class StringToDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateTime.TryParse(reader.GetString(), out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new JsonException("Invalid DateTime format");
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).UtcDateTime;
        }
        else
        {
            throw new JsonException("Unexpected token type for DateTime");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss")); // Adjust the format as needed
    }
}

public class StringToTimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (TimeSpan.TryParse(reader.GetString(), out TimeSpan timeSpan))
            {
                return timeSpan;
            }
            else
            {
                throw new JsonException("Invalid TimeSpan format");
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return TimeSpan.FromMilliseconds(reader.GetInt64());
        }
        else
        {
            throw new JsonException("Unexpected token type for TimeSpan");
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString()); // Adjust the format as needed
    }
}

public class UnixTimestampToDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime;
        }

        throw new JsonException("Unexpected token type for TimeSpan");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString()); // Adjust the format as needed
    }
}

public class SecondsToDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return DateTime.UtcNow.AddSeconds(reader.GetInt64());
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateTime.TryParse(reader.GetString(), out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new JsonException("Invalid DateTime format");
            }
        }
        throw new JsonException("Unexpected token type for TimeSpan");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString()); // Adjust the format as needed
    }
}