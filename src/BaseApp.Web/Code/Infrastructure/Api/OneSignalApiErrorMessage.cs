using System.Collections.Generic;
using Newtonsoft.Json;

namespace BaseApp.Web.Code.Infrastructure.Api
{
    public class OneSignalApiErrorMessage
    {
        [JsonProperty("errors")]
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}
