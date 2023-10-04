using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenu<TAttr>: IMenu where TAttr : Attribute
    {
        new IEnumerable<IMenuItem<TAttr>> Items { get; }
    }
}
