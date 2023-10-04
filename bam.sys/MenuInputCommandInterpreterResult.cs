using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuInputCommandInterpreterResult : IMenuInputCommandInterpreterResult
    {
        public IEnumerable<IMenuItemRunResult?> MenuItemRunResults
        {
            get;
            set;
        }

        protected MenuInputCommandInterpreterResult AddResult(object result)
        {
            throw new NotImplementedException();
        }

        public static MenuInputCommandInterpreterResult FromValues(IMenuItem menuItem, IMenuInput menuInput, params object[] value)
        {
            MenuInputCommandInterpreterResult result = new MenuInputCommandInterpreterResult();
            foreach(object item in value)
            {

            }

            return result;
        }


    }
}
