using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fora.Seeker.Web.Infrastructure.Edgar;

/// <summary>
/// Custom JSON converter that handles conversion of string values to integers.
/// This is needed because the Edgar API returns CIK values as strings (e.g., "0001858912")
/// but we want to store them as integers in our model.
/// </summary>
public class StringToIntConverter : JsonConverter<int>
{
  public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType == JsonTokenType.String)
    {
      string? stringValue = reader.GetString();
      if (int.TryParse(stringValue, out int result))
      {
        return result;
      }
      throw new JsonException($"Unable to convert \"{stringValue}\" to Int32.");
    }
    else if (reader.TokenType == JsonTokenType.Number)
    {
      return reader.GetInt32();
    }

    throw new JsonException($"Unexpected token type: {reader.TokenType}");
  }

  public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
  {
    writer.WriteNumberValue(value);
  }
}
