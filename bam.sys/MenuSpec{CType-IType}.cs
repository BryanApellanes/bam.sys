using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuSpec<CType, IType> : MenuSpec
    {
        public MenuSpec()
        { 
            this.ContainerType = typeof(CType);
            this.ItemAttributeType = typeof(IType);
        }
    }
}
