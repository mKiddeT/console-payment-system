using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class NoneExistingItemException : Exception
    {
        public NoneExistingItemException()
        {
        }

        public NoneExistingItemException(string message) : base(message)
        {
        }

        public NoneExistingItemException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
