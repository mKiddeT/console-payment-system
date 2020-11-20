using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class StregSystemCLI : IStregSystemUI
    {
        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"User [{username}] could not be found.");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"Product [{product}] could not be found.");
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine($"User information: {user}, username: {user.UserName}, balance: {user.GetBalance()}.");
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"{command} has to many arguments.");
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"The command: {adminCommand} could not be recognized.");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine($"User: {transaction.User} bought: {transaction.Product}.");
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            Console.WriteLine($"User: {transaction.User} bought: {count} {transaction.Product}.");
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine($"User: {user} has insufficient cash to buy: {product}");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine($"Error: {errorString}");
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
