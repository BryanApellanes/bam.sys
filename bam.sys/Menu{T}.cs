using Bam.Net;
using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bam.Net.Analytics.Diff;

namespace Bam.Testing.Menu
{
    public class Menu<TAttr> : Menu, IMenu<TAttr> where TAttr : Attribute
    {
        public Menu(Type type, IMenuItemProvider menuItemProvider, IMenuItemSelector menuItemSelector, IMenuItemRunner menuItemRunner) : base(type, typeof(TAttr), menuItemProvider, menuItemSelector, menuItemRunner)
        {
            this.MenuItemProvider = new MenuItemProvider<TAttr>();
        }

        protected new IMenuItemProvider<TAttr> MenuItemProvider
        {
            get;
            set;
        }

        IEnumerable<IMenuItem<TAttr>> _items;
        public new IEnumerable<IMenuItem<TAttr>> Items
        {
            get
            {
                if (_items == null)
                {
                    IMenuItem<TAttr>[] items = this.MenuItemProvider.GetMenuItems(this.ContainerType).ToArray();
                    if (items.Length > 0)
                    {
                        items[0].Selected = true;
                    }
                    _items = items;
                }
                return _items;
            }
        }

        protected static new string GetDisplayName(Type type)
        {
            Args.ThrowIfNull(type, nameof(type));

            string displayName = type.Name;
            if (type.HasCustomAttributeOfType(out TAttr attr))
            {
                if (attr.TryGetPropertyValue("DisplayName", type.Name, out string name))
                {
                    displayName = name;
                }            
            }

            return displayName;
        }

        protected static new string GetSelector(Type type)
        {
            Args.ThrowIfNull(type, nameof(type));

            string selector = type.Name;
            if(type.HasCustomAttributeOfType(out TAttr attr))
            {
                if(attr.TryGetPropertyValue("Selector", type.Name, out string s))
                {
                    selector = s;
                }
            }

            return selector;
        }

        protected static new string GetHeaderText(Type type)
        {
            Args.ThrowIfNull(type, nameof(type));

            string header = string.Empty;
            if(type.HasCustomAttributeOfType(out TAttr attr))
            {
                if(attr.TryGetPropertyValue("Header", string.Empty, out string text))
                {
                    header = text;
                }
            }

            return header;
        }

        protected static new string GetFooterText(Type type)
        {
            Args.ThrowIfNull(type, nameof(type));

            string footer = string.Empty;
            if (type.HasCustomAttributeOfType(out TAttr attr))
            {
                if (attr.TryGetPropertyValue("Footer", string.Empty, out string text))
                {
                    footer = text;
                }
            }

            return footer;
        }
    }
}
