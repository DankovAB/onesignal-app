using System;
using Newtonsoft.Json;

namespace BaseApp.Application.App.Models
{
    public class UpdateAppRequest : CreateAppRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}