using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuInputOutputLoop
    {
        event EventHandler<MenuInputOutputLoopEventArgs> Starting;

        event EventHandler<MenuInputOutputLoopEventArgs> ReadingInputComplete;
        event EventHandler<MenuInputOutputLoopEventArgs> ProcessingInput;

        event EventHandler<MenuInputOutputLoopEventArgs> Ending;

        event EventHandler<MenuItemRunEventArgs> MenuItemRunStarted;
        event EventHandler<MenuItemRunEventArgs> MenuItemRunComplete;

        IMenuManager MenuManager { get; }
        void Start();
        void Start(IMenuInputReader menuInputReader);
        IMenuManager End();
        IMenuManager End(IMenuInputReader menuInputReader);
    }
}
