using Bam.Net;
using Bam.Net.Logging;
using Bam.Sys.Console;
using Bam.Testing.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuManager : IMenuManager
    {
        public MenuManager(IMenuRenderer menuRenderer, IMenuHeaderRenderer menuHeaderRenderer, IMenuFooterRenderer menuFooterRenderer, IMenuProvider menuProvider, IMenuInputReader menuInputReader,  IMenuInputCommandInterpreter menuInputCommandInterpreter, IMenuItemRunner menuItemRunner, IMenuItemRunResultRenderer menuItemRunResultRenderer)
        {
            this.MenusBySelector = new Dictionary<string, IMenu>();
            this.Menus = new List<IMenu>();
            this.MenuRenderer = menuRenderer;
            this.MenuHeaderRenderer = menuHeaderRenderer;
            this.MenuFooterRenderer = menuFooterRenderer;
            this.MenuProvider = menuProvider;
            this.MenuInputReader = menuInputReader;
            this.MenuInputCommandInterpreter = menuInputCommandInterpreter;
            this.MenuItemRunner = menuItemRunner;
            this.MenuItemRunResultRenderer = menuItemRunResultRenderer;

            this.StateUpdating += OnStateUpdating;
            this.DuplicateMenuSelectorSpecified += OnDuplicateMenuSelectorSpecified;
        }

        private void OnDuplicateMenuSelectorSpecified(object? sender, DuplicateMenuSelectorEventArgs e)
        {
            Log.Warn("Duplicate menu selectors specified: [:{0}] ({1}) and [:{2}] ({3})", e.FirstMenu.Selector, e.FirstMenu.Name, e.SecondMenu.Selector, e.SecondMenu.Name);
        }

        public event EventHandler<MenuEventArgs> MenuItemSelected;
        public event EventHandler<MenuManagerUpdateStateEventArgs> StateUpdating;
        public event EventHandler<MenuManagerUpdateStateEventArgs> StateUpdated;
        public event EventHandler<DuplicateMenuSelectorEventArgs> DuplicateMenuSelectorSpecified;
        protected IMenuRenderer MenuRenderer { get; set; }
        protected IMenuHeaderRenderer MenuHeaderRenderer { get; set; }
        protected IMenuFooterRenderer MenuFooterRenderer { get; set; }

        protected IMenuProvider MenuProvider { get; set; }
        protected IMenuInputReader MenuInputReader { get; set; }
        protected IMenuInputCommandInterpreter MenuInputCommandInterpreter { get; set; }
        protected IMenuItemRunner MenuItemRunner { get; set; }
        protected IMenuItemRunResultRenderer MenuItemRunResultRenderer { get; set; }

        private void SetMenuIndex()
        {
            if (CurrentMenu != null)
            {
                for (int i = 0; i < Menus.Count; i++)
                {
                    if (Menus[i] == CurrentMenu)
                    {
                        CurrentMenuIndex = i;
                        break;
                    }
                }
            }
        }

        public void AddMenu(IMenu menu)
        {
            if (menu == null)
            {
                return;
            }
            if (MenusBySelector.ContainsKey(menu.Selector))
            {
                DuplicateMenuSelectorSpecified?.Invoke(this, new DuplicateMenuSelectorEventArgs
                {
                    FirstMenu = MenusBySelector[menu.Selector],
                    SecondMenu = menu
                });
                MenusBySelector[menu.Selector] = menu;
            }
            else
            {
                MenusBySelector.Add(menu.Selector, menu);
            }

            if (!Menus.Contains(menu))
            {
                Menus.Add(menu);
            }
        }

        static ConsoleKey _exitKey = ConsoleKey.Escape;
        public static ConsoleKey DefaultExitKey
        {
            get
            {
                return _exitKey;
            }
            set
            {
                _exitKey = value;
            }
        }

        IMenu? _currentMenu;
        public IMenu? CurrentMenu 
        {
            get
            {
                if(_currentMenu == null)
                {
                    if (Menus.Any())
                    {
                        _currentMenu = Menus.FirstOrDefault();
                    }
                    else
                    {
                        _currentMenu = MenuProvider.GetDefaultMenu();
                        if(_currentMenu != null)
                        {
                            AddMenu(_currentMenu);
                        }
                    }
                    SetMenuIndex();
                }
                return _currentMenu;
            }
            private set
            {
                _currentMenu = value;
                SetMenuIndex();
            }
        }

        public int CurrentMenuIndex
        {
            get;
            private set;
        }

        public Dictionary<string, IMenu> MenusBySelector { get; private set; }
        public List<IMenu> Menus { get; private set; }

        public void AddMenuItemRunStartedHandler(EventHandler<MenuItemRunEventArgs> handler)
        {
            foreach (IMenu menu in Menus)
            {
                menu.SubscribeOnce(nameof(menu.MenuItemRunStarted), handler);
            }
        }

        public void AddMenuItemRunCompleteHandler(EventHandler<MenuItemRunEventArgs> handler)
        {
            foreach (IMenu menu in Menus)
            {
                menu.SubscribeOnce(nameof(menu.MenuItemRunComplete), handler);
            }
        }

        public void AddMenuItemSelectedHandler(EventHandler<MenuEventArgs> handler)
        {
            foreach(IMenu menu in Menus)
            {
                menu.MenuItemSelected += handler;
            }
        }

        public void AddMenuItemSelectionChangedHandler(EventHandler<MenuEventArgs> handler)
        {
            foreach(IMenu menu in Menus)
            {
                menu.MenuItemSelectionChanged += handler;
            }
        }

        public void LoadMenus()
        {
            if (MenuSpecs.LoadList.Any())
            {
                LoadMenus(MenuSpecs.LoadList);
            }
            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            if(entryAssembly != null)
            {
                LoadMenus(entryAssembly);
            }
        }

        public void LoadMenus(Assembly assembly)
        {
            LoadMenus(MenuSpecs.Scan(assembly));
        }

        public void LoadMenus(IEnumerable<MenuSpecs> menuSpecs)
        {
            foreach(MenuSpecs menuSpec in menuSpecs)
            {
                foreach(IMenu menu in menuSpec.CreateMenus(this.MenuProvider))
                {
                    this.AddMenu(menu);
                }
            }
        }

        public IMenu? GetMenu(Type type)
        {
            IMenu menu = this.MenuProvider.GetMenu(type);
            if (menu != null)
            {
                if (!Menus.Contains(menu))
                {
                    AddMenu(menu);
                }
            }

            return menu;
        }

        public IMenu? GetMenu(string selector)
        {
            IMenu? menu = this.MenuProvider.GetMenu(selector);
            if (menu != null)
            {
                if (!Menus.Contains(menu))
                {
                    AddMenu(menu);
                }
            }

            return menu;
        }

        public IMenu? AddMenu(Type type)
        {
            IMenu? menu = GetMenu(type);
            if (menu != null)
            {
                if(!Menus.Contains(menu))
                {
                    AddMenu(menu);
                }
            }

            return menu;
        }

        public IMenuItem? GetSelectedMenuItem()
        {
            if (CurrentMenu != null && CurrentMenu.SelectedItem != null)
            {
                return CurrentMenu.SelectedItem;
            }
            return null;
        }

        public IMenu? SelectMenu(string selector)
        {
            IMenu? menu = GetMenu(selector);
            if (menu != null)
            {
                menu.Selected = true;
                foreach (IMenu existingMenu in Menus)
                {
                    existingMenu.Selected = false;
                }
                this.CurrentMenu = menu;
            }

            return menu;
        }

        public IMenu? SelectNextMenu()
        {
            int next = this.CurrentMenuIndex + 1;
            if (next < Menus.Count)
            {
                CurrentMenu = Menus[this.CurrentMenuIndex + 1];
            }
            return CurrentMenu;
        }

        public IMenu? SelectPreviousMenu()
        {
            if (CurrentMenuIndex > 0)
            {
                CurrentMenu = Menus[CurrentMenuIndex - 1];
            }
            return CurrentMenu;
        }

        public IMenu? SelectMenu(IMenu menu)
        {
            CurrentMenu = menu;
            return menu;
        }

        public IMenu? SelectMenu(IMenuInput menuInput)
        {
            if (menuInput.NextMenu)
            {
                return SelectNextMenu();
            }
            else if (menuInput.PreviousMenu)
            {
                return SelectPreviousMenu();
            }

            return CurrentMenu;
        }

        public IMenuItem? SelectMenuItem(IMenuInput menuInput)
        {
            return CurrentMenu?.SelectItem(menuInput);
        }

        public IMenuItem? SelectMenuItem(IMenu menu, IMenuInput menuInput)
        {
            return menu.SelectItem(menuInput);
        }

        public void RenderMenu()
        {
            if (CurrentMenu == null)
            {
                throw new ArgumentNullException(nameof(CurrentMenu));
            }

            RenderMenu(CurrentMenu);
        }

        protected virtual void UpdateState(IMenu? menu, IMenuInput menuInput)
        {
            Args.ThrowIfNull(menu, nameof(menu));

            StateUpdating?.Invoke(this, new MenuManagerUpdateStateEventArgs
            {
                Menu = menu,
                MenuInput = menuInput                
            });            

            StateUpdated?.Invoke(this, new MenuManagerUpdateStateEventArgs
            {
                Menu = menu,
                MenuInput = menuInput
            });
        }

        protected virtual void OnStateUpdating(object? sender, MenuManagerUpdateStateEventArgs e)
        {
            if (e.MenuInput.IsMenuItemNavigation || e.MenuInput.ItemNumber > 0)
            {
                SelectMenuItem(e.MenuInput);
            }
            else if (e.MenuInput.IsMenuNavigation)
            {
                SelectMenu(e.MenuInput);
            }
            else if (e.MenuInput.IsSelector)
            {
                if (IsMenuSelector(e.MenuInput, out IMenu? menu))
                {
                    if (menu != null)
                    {
                        SelectMenu(menu);
                        e.MenuInput.Input.Clear();
                    }
                }
                else
                {
                    SelectMenuItem(e.MenuInput);
                }
            }
        }

        protected bool IsMenuItemSelector(IMenu menu, IMenuInput menuInput, out IMenuItem? menuItem)
        {
            menuItem = menu.SelectItem(menuInput.Selector);
            return menuItem != null;
        }

        public bool IsMenuSelector(IMenuInput menuInput, out IMenu? menu)
        {
            menu = GetMenu(menuInput.Selector);
            return menu != null;
        }

        protected IMenu[] GetOtherMenus(IMenu menu)
        {
            return Menus.Where(otherMenu => otherMenu != menu).ToArray();
        }

        public void RerenderMenu(IMenuInput menuInput)
        {
            this.UpdateState(CurrentMenu, menuInput);
            if (CurrentMenu != null && menuInput != null)
            {                
                this.RerenderMenu(CurrentMenu, menuInput);
            }
        }

        protected void RerenderMenu(IMenu menu, IMenuInput menuInput)
        {
            this.MenuRenderer.RerenderMenu(menu, menuInput, GetOtherMenus(menu));
        }

        public void RenderMenu(IMenu menu)
        {
            this.MenuRenderer.RenderMenu(menu, GetOtherMenus(menu));
        }

        public IMenuItemRunResult? RunMenuItem(IMenuInput menuInput)
        {
            if (CurrentMenu != null)
            {
                return this.CurrentMenu.RunItem(menuInput);
            }

            return new MenuItemRunResult { Message = "No menu selected" };
        }

        public IMenuManager StartInputOutputLoop()
        {
            this.LoadMenus();
            MenuInputOutputLoop loop = new MenuInputOutputLoop(this, this.MenuInputReader, this.MenuInputCommandInterpreter, this.MenuItemRunResultRenderer);
            loop.Ending += (sender, args) => Environment.Exit(args.MenuInput.ExitCode);
            loop.Start();

            return this;
        }
    }
}
