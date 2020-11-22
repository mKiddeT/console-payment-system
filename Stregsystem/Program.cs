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
            IStregSystemUI ui = new StregSystemCLI(stregSystem);
            StregSystemController controller = new StregSystemController(stregSystem, ui);
            
            ui.Start();
        }
    }
}
