using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public abstract class MenuRenderer : IMenuRenderer
    {
        public MenuRenderer(IMenuHeaderRenderer headerRenderer, IMenuFooterRenderer footerRenderer, IMenuInputReader inputReader)
        {
            this.HeaderRenderer = headerRenderer;
            this.FooterRenderer = footerRenderer;
            this.InputReader = inputReader;
        }

        protected IMenuHeaderRenderer HeaderRenderer { get; private set; }
        protected IMenuFooterRenderer FooterRenderer { get; private set; }

        protected IMenuInputReader InputReader
        {
            get;
            private set;
        }

        protected abstract void RenderItems(IMenu menu);

        public abstract void RerenderMenu(IMenu menu, IMenuInput menuInput, params IMenu[] otherMenus);

        public abstract void RenderMenu(IMenu menu, params IMenu[] otherMenus);
    }
}
