using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuInputParser
    {
        ITypedArgumentProvider TypedArgumentProvider { get; }
        object?[] GetMethodParameters(IMenuItem menuItem, IMenuInput menuInput);
    }
}
