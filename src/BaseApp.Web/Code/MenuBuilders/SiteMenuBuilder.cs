using System.Collections.Generic;
using BaseApp.Web.Code.Infrastructure.Menu;
using BaseApp.Web.Code.Infrastructure.Menu.Models;
using BaseApp.Web.Controllers;

namespace BaseApp.Web.Code.MenuBuilders
{
    public class SiteMenuBuilder : MenuBuilderBase
    {
        public SiteMenuBuilder(MenuBuilderArgs args) : base(args)
        {
        }

        protected override List<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>
            {
                GetHomeMenuItem(),
            };

            return menuItems;
        }
      
        private MenuItem GetHomeMenuItem()
        {
            return new("Home")
            {
                MenuUrlInfo = UrlFactory.Create<HomeController>(m => m.Index())
            };
        }
    }
}