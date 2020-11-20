using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stregsystem.ProgramFiles;

namespace Stregsystem 
{
    class Program 
    {
        static void Main(string[] args) 
        {
            IStregSystem stregSystem = new StregSystem();

            stregSystem.BuyProduct(stregSystem.GetUserByUsername("rking"), stregSystem.GetProductByID(14));

        }
    }
}
