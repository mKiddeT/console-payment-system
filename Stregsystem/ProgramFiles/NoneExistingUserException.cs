using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class NoneExistingUserException : Exception
    {
        public NoneExistingUserException()
        {
        }

        public NoneExistingUserException(string message) : base(message)
        {
        }

        public NoneExistingUserException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
