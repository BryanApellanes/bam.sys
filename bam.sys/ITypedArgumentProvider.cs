using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface ITypedArgumentProvider
    {
        object? GetTypedArgument(ParameterInfo parameter, string input);
    }
}
