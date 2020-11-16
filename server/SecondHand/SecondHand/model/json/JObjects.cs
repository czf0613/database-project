using System;
using System.Text.Json.Serialization;

namespace SecondHand.model.json
{
    public enum Gender
    {
        MALE, FEMALE, UNKNOWN, SECRET
    }

    public class StudentInfo
    {
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Phone { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public string SerialNumber { get; set; }
        public string College { get; set; }
        public string Major { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Dormitory { get; set; }
    }
}
