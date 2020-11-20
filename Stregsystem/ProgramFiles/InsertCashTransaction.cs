using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(User user, decimal amount) : base(user, amount)
        {
        }

        public override void Execute()
        {
            User.AddBalance(Amount);
        }

        public override string ToString()
        {
            return $"{ID}: Deposit of {Amount} to {User}.";
        }
    }
}
