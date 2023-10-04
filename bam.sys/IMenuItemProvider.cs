using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuItemProvider
    {
        IEnumerable<IMenuItem> GetMenuItems(object instance);
        IEnumerable<IMenuItem> GetMenuItems(Type containerType);
        IEnumerable<IMenuItem<T>> GetMenuItems<T>(Type containerType) where T : Attribute;
        IEnumerable<IMenuItem> GetMenuItems(Type containerType, Type itemAttributeType);
    }
}
