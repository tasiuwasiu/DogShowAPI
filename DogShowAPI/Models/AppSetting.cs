using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class AppSetting
    {
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public JsonObject<string[]> SettingValue { get; set; }
        public byte[] SettingData { get; set; }
    }
}
