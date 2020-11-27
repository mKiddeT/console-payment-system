using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class StregSystem : IStregSystem
    {
        public IEnumerable<Product> AllProducts { get; }
        public IEnumerable<Product> ActiveProducts { get; set; }
        public List<User> Users { get; }
        public List<Transaction> Transactions { get; }

        public event UserBalanceNotification UserBalanceWarning;
        private decimal _balanceWarning = 50M;

        public StregSystem()
        {
            Users = ReadUserList(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\\users.csv")).ToList();
            AllProducts = ReadProductList(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\products.csv"));
            UpdateActiveProductList();

            Transactions = new List<Transaction>();
        }

        public void UpdateActiveProductList()
        {
            ActiveProducts = AllProducts.Where(p => p.Active == true);
        }

        public InsertCashTransaction AddCreditsToAccount(User user, int amount)
        {
            InsertCashTransaction ict = new InsertCashTransaction(user, amount);
            ExecuteTransaction(ict);
            return ict;
        }

        public BuyTransaction BuyProduct(User user, Product product)
        {
            BuyTransaction bt = new BuyTransaction(user, product);
            ExecuteTransaction(bt);
            return bt;
        }

        private void ExecuteTransaction(Transaction transaction)
        {
            transaction.Execute();

            if (transaction.User.Balance < _balanceWarning)
                UserBalanceWarning?.Invoke(transaction.User);

            Transactions.Add(transaction);
        }

        public Product GetProductByID(int id)
        {
            foreach (Product product in AllProducts)
            {
                if (product.ID == id)
                    return product;
            }

            throw new NoneExistingProductException($"{id}");
        }

        public IEnumerable<Transaction> GetTransactions(User user, int count)
        {
            List<Transaction> tList = new List<Transaction>();
            foreach (Transaction transaction in Transactions)
            {
                if (tList.Count < count && user.Equals(transaction.User))
                    tList.Add(transaction);
                else if (tList.Count == count)
                    return tList;
            }

            return tList;
        }

        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            List<User> uList = new List<User>();
            foreach (User user in Users)
            {
                if (predicate(user))
                    uList.Add(user);
            }

            return uList;
        }

        public User GetUserByUsername(string username)
        {
            foreach (User user in Users)
            {
                if (user.UserName == username)
                    return user;
            }

            throw new NoneExistingUserException($"{username}");
        }

        /// <summary>
        /// Write transaction logs to log file
        /// </summary>
        public void WriteTransactionLogs()
        {
            using (StreamWriter file = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\\Logs.csv"), true))
            {
                foreach (Transaction transaction in Transactions)
                {
                    if (transaction.GetType() == typeof(BuyTransaction))
                    {
                        BuyTransaction t = (BuyTransaction)transaction;
                        file.WriteLine($"{t.ID}, {t.Date}, {t.User}, {t.Product}");
                    }
                    else
                    {
                        file.WriteLine($"{transaction.ID}, {transaction.Date}, {transaction.User}, {transaction.Amount}");
                    }

                }
            }
        }


        /// <summary>
        /// Reads and creates a list of the Product list specified by "path".
        /// </summary>
        /// <param name="path">The path of the product list.</param>
        /// <returns>Returns a list of all products contained in the product list data file. </returns>
        private IEnumerable<Product> ReadProductList(string path)
        {
            List<Product> pList = new List<Product>();
            string[] sArray = File.ReadAllLines(path).Skip(1).ToArray();
            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Replace('"'.ToString(), "");
                string s = Regex.Replace(sArray[i], @"<(.|\n)*?>", string.Empty);
                string[] productInformation = s.Split(';');

                try
                {
                    pList.Add(new Product(
                        Convert.ToInt32(productInformation[0]),
                        productInformation[1],
                        Convert.ToDecimal(productInformation[2]),
                        Convert.ToInt32(productInformation[3]) == 1,
                        false));
                }
                catch (BadInfomationException)
                {
                }
                

            }

            return pList;
        }

        /// <summary>
        /// Reads and creates a list of the users specified by "path".
        /// </summary>
        /// <param name="path">The path of the user list.</param>
        /// <returns>Returns a list of users contained in the user list data file.</returns>
        private IEnumerable<User> ReadUserList(string path)
        {
            List<User> uList = new List<User>();
            string[] sArray = File.ReadAllLines(path).Skip(1).ToArray();
            for (int i = 0; i < sArray.Length; i++)
            {
                string[] userInformation = sArray[i].Split(',');

                try
                {
                    uList.Add(new User(
                        userInformation[1],
                        userInformation[2],
                        userInformation[3],
                        userInformation[5],
                        Convert.ToDecimal(userInformation[4])));
                }
                catch (BadInfomationException)
                {
                }
                catch (FormatException)
                {
                }
            }

            return uList;
        }
    }
}
