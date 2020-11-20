using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class BuyTransaction : Transaction
    {
        public Product Product { get; }
        public BuyTransaction(User user, Product product) : base(user, product.Price)
        {
            this.Product = product;
        }

        public override void Execute()
        {
            if (User.GetBalance() >= Amount)
                User.AddBalance(-Amount);
            else if (Product.CanBeBoughtOnCredit)
                User.AddBalance(-Amount);
            else
                throw new InsufficientCreditsException($"{User} has insufficient credits.");
        }

        public override string ToString()
        {
            return $"{ID}: Transaction of {Amount} from {User} for {Product}.";
        }
    }
}
