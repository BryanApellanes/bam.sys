using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenuRenderer
    {
        void RerenderMenu(IMenu menu, IMenuInput menuInput, params IMenu[] otherMenus);
        void RenderMenu(IMenu menu, params IMenu[] otherMenus);
    }
}
