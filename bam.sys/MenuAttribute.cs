using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MenuAttribute : Attribute
    {
        public MenuAttribute() { }
        public MenuAttribute(string name) 
        {
            this.Name = name;
            this.DisplayName = name;
        }

        public MenuAttribute(string name, string description) : this(name)
        {
            this.Description = description;
        }

        public Type ItemAttributeType { get; protected set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public string Selector { get; set; }
        public string HeaderText { get; set; }
        public string FooterText { get; set; }
    }
}
