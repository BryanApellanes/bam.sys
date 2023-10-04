using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuProvider
    {
        event EventHandler<DuplicateMenuSelectorEventArgs> DuplicateMenuSelectorSpecified;

        IMenu? GetDefaultMenu();
        IEnumerable<IMenu> GetMenus();
        IEnumerable<IMenu> GetMenus(Assembly assembly);
        IEnumerable<IMenu<TAttr>> GetMenus<TAttr>(Assembly assembly) where TAttr : Attribute;
        IMenu GetMenu(Type type);
        bool HasMenu(string selector);
        IMenu GetMenu(string selector);
        IMenu<TAttr> GetMenu<TAttr>(Type containerType) where TAttr : Attribute;
        IMenu<TAttr> CreateMenu<TAttr>(Type containerType) where TAttr : Attribute;

        IEnumerable<IMenu> CreateMenus(MenuSpecs menuSpec);

        IMenu CreateMenu(Type containerType, Type itemAttributeType);
    }
}
