using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class Product
    {
        public int ID { get; }
        public string Name { get; }
        public decimal Price { get; }
        public bool Active { get; }
        public bool CanBeBoughtOnCredit { get; }

        public Product(int id, string name, decimal price, bool active, bool canBeBoughtOnCredit)
        {
            this.ID = id;
            this.Name = name;
            this.Price = price;
            this.Active = active;
            this.CanBeBoughtOnCredit = canBeBoughtOnCredit;
        }

        public override string ToString()
        {
            return $"{ID} {Name} {Price}";
        }
    }
}
