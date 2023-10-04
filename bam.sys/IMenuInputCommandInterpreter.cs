using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuInputCommandInterpreter
    {
        bool InterpretInput(IMenuManager menuManager, IMenuInput menuInput, out IMenuInputCommandInterpreterResult result);
    }
}
