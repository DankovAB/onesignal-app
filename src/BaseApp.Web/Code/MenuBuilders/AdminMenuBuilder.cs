using BaseApp.Web.Areas.Admin.Controllers;
using BaseApp.Web.Code.Infrastructure.Menu;
using BaseApp.Web.Code.Infrastructure.Menu.Models;
using System.Collections.Generic;

namespace BaseApp.Web.Code.MenuBuilders
{
    public class AdminMenuBuilder : MenuBuilderBase
    {
        public AdminMenuBuilder(MenuBuilderArgs args) : base(args)
        {
        }

        protected override List<MenuItem> GetMenuItems()
        {
            var items = new List<MenuItem>
            {
                GetAllAppsItem(),
            };

            return items;
        }
        
        private MenuItem GetAllAppsItem()
        {
            return new("Apps")
            {
                MenuUrlInfo = UrlFactory.CreateAdmin<AppController>(m => m.Index())

            };
        }
    }
}