using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class BadInfomationException : Exception
    {
        public BadInfomationException()
        {
        }

        public BadInfomationException(string message) : base(message)
        {
        }

        public BadInfomationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
