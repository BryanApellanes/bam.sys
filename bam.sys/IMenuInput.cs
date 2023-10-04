using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuInput
    {
        StringBuilder Input { get; }
        bool Exit { get; }
        int ExitCode { get; }
        bool Enter { get; }

        bool IsMenuItemNavigation { get; }        
        bool IsMenuNavigation { get; }
        bool IsSelector { get; }
        string Value { get; }
        string Selector { get; }
        int ItemNumber { get; }

        bool NextItem { get; }
        bool PreviousItem { get; }

        bool NextMenu { get; }

        bool PreviousMenu { get; }
    }
}
