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

        public event StregSystemEvent CommandEntered;

        private bool _isRunning;

        public StregSystemCLI(IStregSystem stregSystem)
        {
            this.stregSystem = stregSystem;
        }

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
            Console.WriteLine($"Username: {user.UserName}, Name: {user.FirstName} {user.LastName}, Balance: {user.Balance}");
            foreach (Transaction transaction in stregSystem.GetTransactions(user, 10).Reverse())
            {
                Console.WriteLine(transaction);
            }
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"[{command}] has to many arguments.");
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"The admin command: [{adminCommand}] could not be recognized.");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine($"User: {transaction.User.UserName} has bought: [{transaction.Product}].");
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            Console.WriteLine($"User: {transaction.User.UserName} has bought: [{count} x {transaction.Product}].");
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine($"User: {user.UserName} does not have enough credits to buy: {product.ID} {product.Name}");
        }

        public void DisplayInsufficientCash(User user, int count, Product product)
        {
            Console.WriteLine($"User: {user.UserName} has insufficient cash to buy: {count} x {product.ID} {product.Name}");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine($"Error: {errorString}");
        }

        private void DisplayUserLowBalanceWarning(User user)
        {
            int currentCursorPos = Console.CursorTop;
            Console.SetCursorPosition(0, stregSystem.ActiveProducts.Count() + 5);
            Console.WriteLine($"Low balance warning: [{user.UserName}] has low balance remaining: {user.Balance}.");
            Console.SetCursorPosition(0, currentCursorPos);
        }

        public void Start()
        {
            Console.Clear();
            PrintProductList();
            if (!_isRunning)
                stregSystem.UserBalanceWarning += DisplayUserLowBalanceWarning;

            _isRunning = true;
            while (_isRunning)
            {
                ClearConsoleLines(stregSystem.ActiveProducts.Count()+3);
                PromptForInput();
            }
        }

        public void Close()
        {
            stregSystem.WriteTransactionLogs();
            _isRunning = false;
        }

        private void PrintProductList()
        {
            IEnumerable<Product> pList = stregSystem.ActiveProducts;

            Console.Clear();
            Console.WriteLine($"------------------------Stregsystem------------------------");
            Console.WriteLine($"|{"ID:",-8}|{"NAME:",-40}|{"PRICE:",-7}|");
            foreach (Product p in pList)
            {
                Console.WriteLine($"| {p.ID,-7}| {p.Name,-39}| {p.Price,-6}|");
            }
            Console.WriteLine("-----------------------------------------------------------");
        }

        private void PromptForInput()
        {
            Console.Write("Please enter command: ");
            string input = Console.ReadLine();

            PrintProductList();
            Console.WriteLine();

            StregSystemEventArgs command = new StregSystemEventArgs(input);
            CommandEntered?.Invoke(command);
        }

        /// <summary>
        /// Clear the indicated lines and sets the current cursor position to the first line afterwards.
        /// </summary>
        /// <param name="lines">The lines to clear</param>
        private void ClearConsoleLines(params int[] lines)
        {
            foreach (var line in lines)
            {
                Console.SetCursorPosition(0, line);
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(0,lines[0]);
        }
    }
}
