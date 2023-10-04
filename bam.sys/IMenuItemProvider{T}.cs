using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuItemProvider<TAttr> : IMenuItemProvider where TAttr : Attribute
    {
        new IEnumerable<IMenuItem<TAttr>> GetMenuItems(Type type);
    }
}
