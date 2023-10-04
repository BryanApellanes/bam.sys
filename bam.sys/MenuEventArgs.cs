using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuEventArgs : EventArgs
    {
        public MenuEventArgs() { }
        public IMenu Menu { get; set; }
        public IMenuItem? PreviousMenuItem { get; set; }
        public IMenuItem? MenuItem { get; set; }
    }
}
