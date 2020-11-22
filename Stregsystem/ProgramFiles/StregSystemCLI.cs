using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class StregSystemCLI : IStregSystemUI
    {
        private IStregSystem stregSystem;

        public StregSystemCLI(IStregSystem stregSystem)
        {
            this.stregSystem = stregSystem;
        }

        public event StregSystemEvent CommandEntered;

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
            //TODO: Uendeligt loop indtil Close() bliver kaldt
            PrintProductList();
            PromptForInput();
            PromptForInput();
            PromptForInput();
        }

        public void Close()
        {
            //TODO: Stop applikationen
        }

        private void PrintProductList()
        {
            IEnumerable<Product> pList = stregSystem.ActiveProducts;

            foreach (Product p in pList)
            {
                Console.WriteLine(p);
            }
        }

        private void PromptForInput()
        {
            string input;
            Console.Write("Please enter: ");
            input = Console.ReadLine();


            StregSystemEventArgs command = new StregSystemEventArgs(input);
            CommandEntered.Invoke(command);
        }
    }
}
