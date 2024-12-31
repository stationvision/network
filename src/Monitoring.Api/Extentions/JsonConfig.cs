using Monitoring.Core.Queries;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monitoring.Api.Extentions
{
    public class JsonConfig : JsonConverter<QueryResult>
    {
        public override QueryResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Deserialization is not implemented.");
        }

        public override void Write(Utf8JsonWriter writer, QueryResult value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.RowData, options);
        }
    }
}
