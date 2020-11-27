using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class SeasonalProduct : Product
    {
        public DateTime SeasonStartDate { get; }
        public DateTime SeasonEndDate { get; }

        public SeasonalProduct(int id, string name, decimal price, bool active, bool canBeBoughtOnCredit, DateTime seasonStartDate, DateTime seasonEndDate) : base(id, name, price, active, canBeBoughtOnCredit)
        {
            this.SeasonStartDate = seasonStartDate;
            this.SeasonEndDate = seasonEndDate;

            Active = IsSeasonActive();
        }

        public bool IsSeasonActive()
        {
            return DateTime.Now >= SeasonStartDate && DateTime.Now <= SeasonEndDate;
        }
    }
}
