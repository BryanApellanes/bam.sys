using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class NullMenuInputCommandInterpreter : IMenuInputCommandInterpreter
    {
        public bool InterpretInput(IMenuManager menuManager, IMenuInput menuInput, out IMenuInputCommandInterpreterResult result)
        {
            result = new MenuInputCommandInterpreterResult();
            return false;
        }
    }
}
