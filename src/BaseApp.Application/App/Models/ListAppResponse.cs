using System.Collections.Generic;

namespace BaseApp.Application.App.Models
{
    public class ListAppResponse
    {
        public IEnumerable<AppResponse> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
