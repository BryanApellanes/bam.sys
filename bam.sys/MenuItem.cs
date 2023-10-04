using Bam.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuItem : IMenuItem
    {
        public MenuItem() { }

        public MenuItem(MethodInfo method, Type attributeType)
        {
            this.MethodInfo = method;
            this.AttributeType = attributeType;
            this.Attribute = method.GetCustomAttribute(attributeType);
        }

        public MenuItem(object instance, MethodInfo method, Type attributeType) : this(method, attributeType)
        {
            this.Instance = instance;
        }

        public Attribute? Attribute
        {
            get;
            set;
        }

        public Type AttributeType
        {
            get;
            set;
        }

        public string Selector
        {
            get
            {
                string selector = string.Empty;
                Attribute?.TryGetPropertyValue("Selector", MethodInfo.Name, out selector);

                return selector;
            }
        }

        public string DisplayName
        {
            get
            {
                string displayName = string.Empty;
                Attribute?.TryGetPropertyValue("DisplayName", MethodInfo.Name, out displayName);

                return string.IsNullOrEmpty(displayName) ? MethodInfo.Name : displayName;
            }
        }

        public MethodInfo MethodInfo
        {
            get;
            set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public object Instance
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
