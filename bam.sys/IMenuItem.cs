using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuItem
    {
        object Instance { get; set; }
        Attribute? Attribute { get; set; }
        bool Selected { get; set; }
        string Selector { get; }
        string DisplayName { get; }
        MethodInfo MethodInfo { get; }
    }
}
