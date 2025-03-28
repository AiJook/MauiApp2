namespace MauiApp2.Model.StudentNamespace
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters; // สำหรับ IsoDateTimeConverter
    using System.Globalization;
  

    public partial class Student
    {
        [JsonProperty("studentId")]
        public string StudentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; } // เปลี่ยนจาก long เป็น string

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; } // เปลี่ยนจาก long เป็น string
    }

    public partial class Student
    {
        public static List<Student> FromJson(string json) => JsonConvert.DeserializeObject<List<Student>>(json, MauiApp2.Model.StudentNamespace.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<Student> self) => JsonConvert.SerializeObject(self, MauiApp2.Model.StudentNamespace.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
