using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuItemAttribute : Attribute
    {
        public MenuItemAttribute() { }
        public MenuItemAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }

        public MenuItemAttribute(string displayName, string description) : this(displayName)
        {
            this.Description = description;
        }

        public string Selector { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
