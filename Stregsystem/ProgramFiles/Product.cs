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
        public decimal Price { get; private set; }
        public bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }

        public Product(int id, string name, decimal price, bool active, bool canBeBoughtOnCredit)
        {
            this.ID = id;
            if (name != null)
                this.Name = name;
            this.Price = price / 100;  
            this.Active = active;
            this.CanBeBoughtOnCredit = canBeBoughtOnCredit;

            if (name == null)
                throw new BadInfomationException("Name cannot be null");
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }

        public override string ToString()
        {
            return $"{ID}: {Name}: {Price}";
        }
    }
}
