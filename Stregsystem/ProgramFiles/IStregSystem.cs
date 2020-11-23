using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    interface IStregSystem
    {
        IEnumerable<Product> ActiveProducts { get; }
        void UpdateActiveProductList();
        InsertCashTransaction AddCreditsToAccount(User user, int amount);
        BuyTransaction BuyProduct(User user, Product product);
        Product GetProductByID(int id);
        IEnumerable<Transaction> GetTransactions(User user, int count);
        IEnumerable<User> GetUsers(Func<User, bool> predicate);
        User GetUserByUsername(string username);
    }
}
