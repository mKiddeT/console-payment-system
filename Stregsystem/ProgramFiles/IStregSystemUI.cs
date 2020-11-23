using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    interface IStregSystemUI
    {
        event StregSystemEvent CommandEntered;
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayUserInfo(User user);
        void DisplayTooManyArgumentsError(string command);
        void DisplayAdminCommandNotFoundMessage(string adminCommand);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(int count, BuyTransaction transaction);
        void DisplayInsufficientCash(User user, Product product);
        void DisplayInsufficientCash(User user, int count, Product product);
        void DisplayGeneralError(string errorString);
        void Start();
        void Close();
    }

    delegate void StregSystemEvent(StregSystemEventArgs args);
}
