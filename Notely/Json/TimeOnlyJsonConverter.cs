using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notely.Json;

/// <summary>
/// Le convertisseur TimeOnly par défaut de System.Text.Json exige le format "HH:mm:ss"
/// et rejette "HH:mm" — or c'est exactement ce que renvoie un &lt;input type="time"&gt;
/// HTML sans attribut "step", ce qui provoquait un 400 sur toute création/modification
/// d'événement ou de séance. TimeOnly.Parse accepte les deux formats nativement.
/// </summary>
public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm:ss";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}
