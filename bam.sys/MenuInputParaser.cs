using Bam.Net;
using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Commandline.Menu
{
    public class MenuInputParaser : IMenuInputParser
    {
        public MenuInputParaser(ITypedArgumentProvider parameterProvider)
        {
            this.TypedArgumentProvider = parameterProvider;
        }

        public ITypedArgumentProvider TypedArgumentProvider { get; set; }

        public virtual object?[] GetMethodParameters(IMenuItem menuItem, IMenuInput menuInput)
        {
            Args.ThrowIfNull(menuItem, "menuItem");
            Args.ThrowIfNull(menuInput, "menuInput");

            ParameterInfo[] parameterInfos = menuItem.MethodInfo.GetParameters();

            if(parameterInfos.Length  == 0 )
            {
                return Array.Empty<object>();
            }

            string[]  inputStrings = menuInput.Value.Split(',');
            if(inputStrings.Length != parameterInfos.Length)
            {
                throw new ArgumentException("argument count mismatch");
            }
            object?[] arguments = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                arguments[i] = TypedArgumentProvider.GetTypedArgument(parameterInfos[i], inputStrings[i]);
            }

            return arguments;
        }
    }
}
