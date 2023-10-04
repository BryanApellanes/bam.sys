using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuSpec
    {
        public MenuSpec() { }

        public MenuSpec(Type containerType) : this(containerType, typeof(MenuItemAttribute))
        {
        }

        public MenuSpec(Type containerType, Type itemAttributeType) 
        {
            this.ContainerType  = containerType;
            this.ItemAttributeType = itemAttributeType;
        }

        public Type ContainerType { get; set; }
        public Type ItemAttributeType { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj == this) return true;
            if(this == null) return false;
            if(obj is MenuSpec menuSpec)
            {
                return menuSpec.ContainerType == ContainerType && menuSpec.ItemAttributeType == ItemAttributeType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                if(ContainerType != null)
                {
                    hash = hash * 23 + ContainerType.GetHashCode();
                }
                if(ItemAttributeType != null)
                {
                    hash = hash * 23 + ItemAttributeType.GetHashCode();
                }
                return hash;
            }
        }
    }
}
