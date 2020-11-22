using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class StregSystemEventArgs : EventArgs
    {
        public string Command;

        public StregSystemEventArgs()
        {
        }

        public StregSystemEventArgs(string command)
        {
            this.Command = command;
        }
    }
}
