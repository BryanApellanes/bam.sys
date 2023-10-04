using Bam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuItemRunner
    {
        IDependencyProvider DependencyProvider { get; }
        IMenuInputParser InputParser { get; set; }
        IMenuItemRunResult RunMenuItem(IMenuItem menuItem, IMenuInput menuInput);
    }
}
