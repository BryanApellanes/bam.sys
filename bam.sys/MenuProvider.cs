using Bam.Net;
using Bam.Testing.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuProvider : IMenuProvider
    {
        private Dictionary<string, IMenu> menusBySelector = new Dictionary<string, IMenu>();

        public MenuProvider(IMenuItemProvider menuItemProvider, IMenuItemSelector menuItemSelector, IMenuItemRunner menuItemRunner)
        {
            this.MenuItemRunner = menuItemRunner;
            this.MenuItemProvider = menuItemProvider;
            this.MenuItemSelector = menuItemSelector;
        }

        public MenuProvider(IMenuItemProvider menuItemProvider, IMenuItemRunner menuItemRunner) : this(menuItemProvider, new MenuItemSelector(), menuItemRunner)
        {
        }

        public event EventHandler<DuplicateMenuSelectorEventArgs> DuplicateMenuSelectorSpecified;
        private void AddMenu(IMenu menu)
        {
            if (menusBySelector.ContainsKey(menu.Selector))
            {
                DuplicateMenuSelectorSpecified?.Invoke(this, new DuplicateMenuSelectorEventArgs
                {
                    FirstMenu = menusBySelector[menu.Selector],
                    SecondMenu = menu
                });

                menusBySelector[menu.Selector] = menu;
            }
            else
            {
                menusBySelector.Add(menu.Selector, menu);
            }
        }

        protected IMenuItemRunner MenuItemRunner
        {
            get;
            set;
        }

        protected IMenuItemProvider MenuItemProvider
        {
            get;
            set;
        }

        protected IMenuItemSelector MenuItemSelector
        {
            get;
            set;
        }

        public virtual IMenu GetMenu(Type containerType)
        {
            return GetMenu<MenuItemAttribute>(containerType);
        }

        public bool HasMenu(string selector)
        {
            return menusBySelector.ContainsKey(selector);
        }

        public IMenu? GetMenu(string selector)
        {
            if (menusBySelector.ContainsKey(selector))
            {
                return menusBySelector[selector];
            }

            return null;
        }

        public IMenu<TAttr> GetMenu<TAttr>(Type containerType) where TAttr : Attribute
        {
            Args.ThrowIfNull(containerType, "type");

            return CreateMenu<TAttr>(containerType);
        }

        public IMenu<TAttr> CreateMenu<TAttr>(Type containerType) where TAttr : Attribute
        {
            Menu<TAttr> menu = new Menu<TAttr>(containerType, this.MenuItemProvider, this.MenuItemSelector, this.MenuItemRunner);

            AddMenu(menu);

            return menu;
        }

        public IEnumerable<IMenu> CreateMenus(MenuSpecs menuSpec)
        {
            foreach(Type itemAttributeType in menuSpec.ItemAttributeTypes)
            {
                yield return CreateMenu(menuSpec.ContainerType, itemAttributeType);
            }
        }

        public IMenu CreateMenu(MenuSpec menuSpec)
        {
            return CreateMenu(menuSpec.ContainerType, menuSpec.ItemAttributeType);
        }

        public IMenu CreateMenu(Type containerType, Type itemAttributeType)
        {
            Menu menu = new Menu(containerType, itemAttributeType, this.MenuItemProvider, this.MenuItemSelector, this.MenuItemRunner);

            AddMenu(menu);

            return menu;
        }

        public IMenu? GetDefaultMenu()
        {
            return GetMenus().FirstOrDefault();
        }

        public IEnumerable<IMenu> GetMenus()
        {
            return GetMenus(Assembly.GetEntryAssembly());
        }

        public virtual IEnumerable<IMenu> GetMenus(Assembly? assembly)
        {
            if (assembly == null)
            {
                return new List<IMenu>();
            }

            return GetMenus<MenuItemAttribute>(assembly);
        }

        public IEnumerable<IMenu<TAttr>> GetMenus<TAttr>(Assembly assembly) where TAttr : Attribute
        {
            foreach (Type type in assembly.GetTypes().Where(t => t.HasCustomAttributeOfType<MenuAttribute>()))
            {
                yield return GetMenu<TAttr>(type);
            }
        }
    }
}
