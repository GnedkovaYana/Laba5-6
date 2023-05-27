using System.Text.Json.Serialization;

namespace Laba5
{
    public class Column
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("isPrimary")]
        public bool IsPrimary { get; set; }

        public override string ToString()
        {
            return Name;
        }

        [JsonIgnore]
        public string TreeViewString => Name + " - " + Type;
    }
}
