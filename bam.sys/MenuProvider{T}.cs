using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuProvider<TAttr> : MenuProvider where TAttr : Attribute
    {
        public MenuProvider(IMenuItemProvider menuItemProvider, IMenuItemSelector menuItemSelector, IMenuItemRunner menuItemRunner) : base(menuItemProvider, menuItemSelector, menuItemRunner)
        {
        }

        public override IMenu GetMenu(Type type)
        {
            return GetMenu<TAttr>(type);
        }

        public override IEnumerable<IMenu> GetMenus(Assembly assembly)
        {
            if (assembly == null)
            {
                return new List<IMenu>();
            }

            return GetMenus<TAttr>(assembly);
        }
    }
}
