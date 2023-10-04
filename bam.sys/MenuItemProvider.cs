using Bam.CommandLine;
using Bam.Net;
using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuItemProvider : IMenuItemProvider
    {
        public IEnumerable<IMenuItem<MenuItemAttribute>> GetMenuItems(Type containerType)
        {
            return GetMenuItems<MenuItemAttribute>(containerType);
        }

        public virtual IEnumerable<IMenuItem> GetMenuItems(object instance)
        {
            foreach(IMenuItem item in GetMenuItems<MenuItemAttribute>(instance.GetType()))
            {
                item.Instance = instance;
                yield return item;
            }
        }

        public virtual IEnumerable<IMenuItem<TAttr>> GetMenuItems<TAttr>(Type containerType) where TAttr : Attribute
        {
            foreach(MethodInfo method in containerType.GetMethods())
            {
                if (method.HasCustomAttributeOfType(out TAttr attribute))
                {
                    yield return new MenuItem<TAttr>(method);
                }
            }
        }

        public virtual IEnumerable<IMenuItem> GetMenuItems(Type containerType, Type itemAttributeType)
        {
            foreach(MethodInfo method in containerType.GetMethods())
            {
                if(method.HasCustomAttributeOfType(itemAttributeType, out object attribute))
                {
                    yield return new MenuItem(method, itemAttributeType)
                    {
                        Attribute = (Attribute)attribute
                    };
                }
            }
        }

        IEnumerable<IMenuItem> IMenuItemProvider.GetMenuItems(Type containterType)
        {
            return GetMenuItems(containterType);
        }
    }
}
