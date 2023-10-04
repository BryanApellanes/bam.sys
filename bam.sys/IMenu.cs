using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public interface IMenu
    {
        event EventHandler<MenuEventArgs> MenuItemSelected;
        event EventHandler<MenuEventArgs> MenuItemSelectionChanged;

        event EventHandler<MenuItemRunEventArgs> MenuItemRunStarted;
        event EventHandler<MenuItemRunEventArgs> MenuItemRunComplete;

        bool Selected { get; set; }
        string Name { get; }
        string DisplayName { get; }
        string Selector { get; }
        string HeaderText { get; }
        string FooterText { get; }

        ConsoleKey ExitKey { get; }

        IMenuItem? SelectedItem { get; }
        int SelectedItemIndex { get; }
        IEnumerable<IMenuItem> Items { get; }

        void UnselectAll();

        IMenuItem? GetItem(string selector);

        IMenuItem? SelectItem(IMenuInput menuInput);
        IMenuItem? SelectItem(string itemSelector);
        IMenuItem? SelectItem(int index);
        IMenuItem? SelectItemNumber(int itemNumber);
        IMenuItem? SelectNextItem();
        IMenuItem? SelectPreviousItem();
        IMenuItemRunResult RunItem(IMenuInput menuInput);
    }
}
