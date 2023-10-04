using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class DuplicateMenuSelectorEventArgs
    {
        public IMenu FirstMenu { get; set; }
        public IMenu SecondMenu { get; set;}
    }
}
