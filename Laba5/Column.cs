﻿using System.Text.Json.Serialization;

namespace Laba5
{
    public class Column
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}