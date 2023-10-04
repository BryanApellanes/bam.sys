using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuItemSelector : IMenuItemSelector
    {
        public event EventHandler<MenuEventArgs> MenuItemSelectionChanged;

        public virtual IMenuItem? SelectMenuItem(IMenu menu, IMenuInput menuInput)
        {
            IMenuItem? currentSelection = menu.SelectedItem;
            IMenuItem? newSelection = null;
            if (menuInput.NextItem)
            {
                newSelection = menu.SelectNextItem();
            }
            else if (menuInput.PreviousItem)
            {
                newSelection = menu.SelectPreviousItem();
            }
            else if (menuInput.ItemNumber > -1)
            {
                newSelection = menu.SelectItemNumber(menuInput.ItemNumber);
                if(newSelection != null)
                {
                    menuInput.Input.Clear();
                }
            }
            else if (menuInput.IsSelector && !string.IsNullOrEmpty(menuInput.Selector))
            {
                newSelection = menu.SelectItem(menuInput.Selector);
                if(newSelection != null)
                {
                    menuInput.Input.Clear();
                }
            }

            if (currentSelection != null)
            {
                if (newSelection != null && currentSelection != newSelection)
                {
                    MenuItemSelectionChanged?.Invoke(this, new MenuEventArgs
                    {
                        Menu = menu,
                        PreviousMenuItem = currentSelection,
                        MenuItem = newSelection,
                    });
                }
            }
            return newSelection;
        }
    }
}
