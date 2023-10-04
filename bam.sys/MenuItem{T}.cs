using Bam.Net;
using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{ 
    public class MenuItem<TAttr> : MenuItem, IMenuItem<TAttr> where TAttr : Attribute
    {
        public MenuItem() { }

        public MenuItem(MethodInfo method)
        {
            this.MethodInfo = method;
            this.Attribute = method.GetCustomAttribute<TAttr>();
        }

        public MenuItem(object instance, MethodInfo method) : this(method)
        {
            this.Instance = instance;
        }

        public new TAttr? Attribute
        {
            get;
            set;
        }

        Attribute? IMenuItem.Attribute
        {
            get
            {
                return Attribute;
            }
            set
            {
                Attribute = (TAttr?)value;
            }
        }
    }
}
