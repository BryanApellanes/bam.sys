using System.Reflection;

namespace Bam.Sys
{
    public interface IMenuManager
    {
        event EventHandler<MenuEventArgs> MenuItemSelected;
        event EventHandler<MenuManagerUpdateStateEventArgs> StateUpdating;
        event EventHandler<MenuManagerUpdateStateEventArgs> StateUpdated;
        event EventHandler<DuplicateMenuSelectorEventArgs> DuplicateMenuSelectorSpecified;

        IMenu? CurrentMenu { get; }
        Dictionary<string, IMenu> MenusBySelector { get; }

        void AddMenuItemSelectedHandler(EventHandler<MenuEventArgs> handler);
        void AddMenuItemSelectionChangedHandler(EventHandler<MenuEventArgs> handler);
        void AddMenuItemRunStartedHandler(EventHandler<MenuItemRunEventArgs> handler);
        void AddMenuItemRunCompleteHandler(EventHandler<MenuItemRunEventArgs> handler);

        void LoadMenus(Assembly assembly);
        void LoadMenus(IEnumerable<MenuSpecs> menuSpecs);

        void AddMenu(IMenu menu);
        IMenu? AddMenu(Type type);
        IMenu? GetMenu(string selector);
        IMenu? GetMenu(Type type);
        void RerenderMenu(IMenuInput menuInput);
        void RenderMenu();
        void RenderMenu(IMenu menu);
        IMenuItemRunResult? RunMenuItem(IMenuInput menuInput);
        IMenuItem? GetSelectedMenuItem();
        IMenu? SelectMenu(string selector);
        IMenu? SelectMenu(IMenu menu);
        IMenu? SelectNextMenu();
        IMenu? SelectPreviousMenu();
        IMenuItem? SelectMenuItem(IMenuInput menuInput);
        IMenuItem? SelectMenuItem(IMenu menu, IMenuInput menuInput);
        IMenuManager StartInputOutputLoop();

    }
}