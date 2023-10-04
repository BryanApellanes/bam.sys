using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MenuAttribute<TAttr> : MenuAttribute where TAttr : Attribute
    {
        public MenuAttribute() 
        {
            this.ItemAttributeType = typeof(TAttr);
        }

        public MenuAttribute(string name): base(name)
        {
            this.ItemAttributeType = typeof(TAttr);
        }
    }
}
