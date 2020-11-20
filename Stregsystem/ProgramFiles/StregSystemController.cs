using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class StregSystemController
    {
        private IStregSystem stregSystem;
        private IStregSystemUI stregSystemUI;

        private Dictionary<string, Func<bool>> _adminCommands = new Dictionary<string, Func<bool>>();

        private bool yes = false;

        public StregSystemController()
        {
            AddCommands();
        }

        private void AddCommands()
        {
            _adminCommands.Add(":quit", () => yes = true);
        }
    }
}
