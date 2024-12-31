using System.Text.Json;

namespace NetworkCommunications.Extentions
{
    public static class JsonSerializerConfig
    {
        public static JsonSerializerOptions DefaultOptions { get; } = new JsonSerializerOptions
        {
            WriteIndented = true,
            // Add more default options here as needed
        };

    }
}
