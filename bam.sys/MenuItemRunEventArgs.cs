using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuItemRunEventArgs : EventArgs
    {
        public MenuItemRunEventArgs() { }

        public IMenu Menu { get; set; }
        public IMenuItem MenuItem { get; set; }
        public IMenuInput MenuInput { get; set; }
        public IMenuItemRunResult Result { get; set; }
    }
}
