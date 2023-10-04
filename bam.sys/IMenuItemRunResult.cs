using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuItemRunResult
    {
        object? Result { get; }
        IMenuInput? MenuInput { get; }
        IMenuItem? MenuItem { get; }
        bool Success { get; }
        string? Message { get; }
        Exception? Exception { get; }
    }
}
