using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    abstract class Transaction
    {
        public string ID { get; }
        public User User { get; }
        public DateTime Date { get; }
        public decimal Amount { get; }

        public Transaction(User user, decimal amount)
        {
            this.ID = Guid.NewGuid().ToString("N");
            this.User = user;
            this.Date = DateTime.Now;
            this.Amount = amount;
        }

        public abstract void Execute();

        public override string ToString()
        {
            return $"{ID} {User} {Amount} {Date}";
        }
    }
}
