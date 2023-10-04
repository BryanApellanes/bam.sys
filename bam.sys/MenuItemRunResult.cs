using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Sys
{
    public class MenuItemRunResult : IMenuItemRunResult
    {
        public object? Result { get; set; } = null;
        public IMenuInput? MenuInput { get; set; }

        public IMenuItem? MenuItem
        {
            get;
            set;
        }

        public bool Success
        {
            get;
            set;
        }

        string? _message;
        public string? Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message) && Exception != null)
                {
                    _message = Exception.Message;
                }
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public Exception? Exception
        {
            get;
            set;
        }
    }
}
