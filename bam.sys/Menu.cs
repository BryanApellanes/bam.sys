using Bam.Net;
using Bam.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Testing.Menu
{
    public class Menu : IMenu
    {
        public const string DefaultHeaderText = "Select an option below:";
        public const string DefaultFooterText = "-----------------------------------";

        public Menu(Type type, Type itemAttributeType, IMenuItemProvider menuItemProvider, IMenuItemSelector menuItemSelector, IMenuItemRunner menuItemRunner)
        {
            this.ContainerType = type;
            this.ItemAttributeType = itemAttributeType;
            this.Name = GetName();
            this.DisplayName = GetDisplayName();
            this.Description = GetDescription();
            this.Selector = GetSelector();
            this.HeaderText = GetHeaderText();
            this.FooterText = GetFooterText();
            this.MenuItemSelector = menuItemSelector;
            this.MenuItemRunner = menuItemRunner;
            this.MenuItemProvider = menuItemProvider;            
            this.ExitKey = MenuManager.DefaultExitKey;

            this.MenuItemSelector.MenuItemSelectionChanged += (sender, args) => this.MenuItemSelectionChanged?.Invoke(sender, args);
        }
        
        public event EventHandler<MenuEventArgs> MenuItemSelected;
        public event EventHandler<MenuEventArgs> MenuItemSelectionChanged;

        public event EventHandler<MenuItemRunEventArgs> MenuItemRunStarted;
        public event EventHandler<MenuItemRunEventArgs> MenuItemRunComplete;

        public IMenuItemSelector MenuItemSelector 
        { 
            get;
            private set; 
        }

        protected static string GetName(Type type)
        {
            Args.ThrowIfNull(type, nameof(type));

            if(type.HasCustomAttributeOfType(out MenuAttribute attribute))
            {
                return attribute.Name ?? type.Name;
            }

            return type.Name;
        }

        protected virtual string GetName()
        {
            return GetMenuAttributeProperty(nameof(Name));
        }

        protected virtual string GetDisplayName()
        {
            return GetMenuAttributeProperty(nameof(DisplayName));
        }

        protected virtual string GetDescription()
        {
            return GetMenuAttributeProperty(nameof(Description));
        }

        protected virtual string GetSelector()
        {
            return GetMenuAttributeProperty(nameof(Selector), GetName().PascalCase(true, " ").CaseAcronym().ToLowerInvariant());
        }

        protected virtual string GetHeaderText()
        {
            return GetMenuAttributeProperty(nameof(HeaderText), DefaultHeaderText);
        }

        protected virtual string GetFooterText()
        {
            return GetMenuAttributeProperty(nameof(FooterText), DefaultFooterText);
        }

        protected string GetMenuAttributeProperty(string propertyName)
        {
            return GetMenuAttributeProperty(this.ContainerType, this.ItemAttributeType, propertyName);
        }

        protected string GetMenuAttributeProperty(string propertyName, string valueIfPropertyMissing)
        {
            return GetMenuAttributeProperty(this.ContainerType, this.ItemAttributeType, propertyName, valueIfPropertyMissing);
        }

        protected static string GetMenuAttributeProperty(Type containerType, Type itemAttributeType, string propertyName)
        {
            return GetMenuAttributeProperty(containerType, itemAttributeType, propertyName, containerType.Name);
        }

        protected static string GetMenuAttributeProperty(Type containerType, Type itemAttributeType, string propertyName, string valueIfPropertyMissing)
        {
            Args.ThrowIfNull(containerType, nameof(containerType));
            Args.ThrowIfNull(itemAttributeType, nameof(itemAttributeType));
            Args.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
            Args.ThrowIfNullOrEmpty(valueIfPropertyMissing, nameof(valueIfPropertyMissing));

            object[] attributes = containerType.GetCustomAttributes(typeof(MenuAttribute), true);
            if (attributes.Length == 0)
            {
                return valueIfPropertyMissing;
            }
            if (attributes.Length == 1)
            {
                if (attributes[0] is MenuAttribute attribute && attribute.TryGetPropertyValue(propertyName, valueIfPropertyMissing, out string value))
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                    return valueIfPropertyMissing;
                }
            }

            foreach (object attribute in attributes)
            {
                if (attribute is MenuAttribute menuAttribute)
                {
                    if (menuAttribute.ItemAttributeType == itemAttributeType &&
                        menuAttribute.TryGetPropertyValue(propertyName, valueIfPropertyMissing, out string value))
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            return value;
                        }
                        return valueIfPropertyMissing;
                    }
                }
            }

            return valueIfPropertyMissing;
        }

        protected Type ContainerType { get; set; }
        protected Type ItemAttributeType { get; set; }
        protected IMenuItemProvider MenuItemProvider
        {
            get;
            set;
        }

        protected IMenuItemRunner MenuItemRunner
        {
            get;
            set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public bool Selected
        { 
            get;
            set; 
        }

        public string Selector
        {
            get;
            private set;
        }

        public string HeaderText
        {
            get;
            private set;
        }

        public string FooterText
        {
            get;
            private set;
        }

        public ConsoleKey ExitKey
        {
            get;
            private set;
        }

        IEnumerable<IMenuItem> _items;
        public IEnumerable<IMenuItem> Items
        {
            get
            {
                if (_items == null)
                {
                    IMenuItem[] items = this.MenuItemProvider.GetMenuItems(this.ContainerType, this.ItemAttributeType).ToArray();
                    if (items.Length > 0)
                    {
                        items[0].Selected = true;
                    }
                    _items = items;
                }
                return _items;
            }
        }

        IMenuItem? _selectedItem;
        public IMenuItem? SelectedItem
        {
            get
            {
                if(_selectedItem == null)
                {
                    _selectedItem = Items.FirstOrDefault(item => item.Selected);

                }
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                if (SelectedItem == null)
                {
                    return -1;
                }
                int index = -1;
                foreach (IMenuItem item in Items)
                {
                    ++index;
                    if (item.Selected)
                    {
                        return index;
                    }
                }
                return -1;
            }
        }

        public IMenuItem? SelectItem(IMenuInput menuInput)
        {
            return this.MenuItemSelector.SelectMenuItem(this, menuInput);
        }

        public IMenuItem? SelectItem(string itemSelector)
        {
            IMenuItem? menuItem = GetItem(itemSelector);
            if (menuItem != null)
            {
                UnselectAll();
                menuItem.Selected = true;
                SelectedItem = menuItem;
                MenuItemSelected?.Invoke(this, new MenuEventArgs
                {
                    Menu = this,
                    MenuItem = menuItem,
                });
            }
            return menuItem;
        }

        public void UnselectAll()
        {
            foreach(IMenuItem menuItem in Items)
            {
                menuItem.Selected = false;
            }
        }

        public IMenuItem? GetItem(string selector)
        {
            return Items.FirstOrDefault(item => item.Selector == selector);
        }

        public IMenuItem? SelectItemNumber(int itemNumber)
        {
            return SelectItem(itemNumber - 1);
        }

        public IMenuItem? SelectItem(int index)
        {
            IMenuItem[] itemArray = Items.ToArray();
            if (itemArray.Length > 0 && index >= 0 && index < itemArray.Length)
            {
                if(SelectedItem != null)
                {
                    SelectedItem.Selected = false;
                }
                IMenuItem menuItem = itemArray[index];
                menuItem.Selected = true;
                SelectedItem = menuItem;
                MenuItemSelected?.Invoke(this, new MenuEventArgs
                {
                    Menu = this,
                    MenuItem = menuItem,
                });
                return menuItem;
            }

            return null;
        }

        public IMenuItem? SelectNextItem()
        {
            return SelectItem(SelectedItemIndex + 1);
        }

        public IMenuItem? SelectPreviousItem()
        {
            return SelectItem(SelectedItemIndex - 1);
        }

        public IMenuItemRunResult RunItem(IMenuInput menuInput)
        {
            IMenuItemRunResult runResult = new MenuItemRunResult()
            {
                Message = "No item selected"
            };

            if (SelectedItem != null)
            {
                MenuItemRunStarted?.Invoke(this, new MenuItemRunEventArgs
                {
                    Menu = this,
                    MenuItem = SelectedItem,
                    MenuInput = menuInput,
                });

                runResult = this.MenuItemRunner.RunMenuItem(SelectedItem, menuInput);

                MenuItemRunComplete?.Invoke(this, new MenuItemRunEventArgs
                {
                    Menu = this,
                    MenuItem = SelectedItem,
                    MenuInput = menuInput,
                    Result = runResult
                });
            }

            return runResult;
        }
    }
}
