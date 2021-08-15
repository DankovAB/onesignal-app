using System.Collections.Generic;

namespace BaseApp.Web.Areas.Admin.Models.App
{
    public class ListAppViewModel
    {
        public IEnumerable<AppViewModel> Items { get; set; } = new List<AppViewModel>();

        public int TotalCount { get; set; }
    }
}