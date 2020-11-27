using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class NoneExistingProductException : Exception
    {
        public NoneExistingProductException()
        {
        }

        public NoneExistingProductException(string message) : base(message)
        {
        }

        public NoneExistingProductException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
