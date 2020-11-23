using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles
{
    class StregSystemController
    {
        private IStregSystem stregSystem;
        private IStregSystemUI ui;

        public StregSystemController(IStregSystem stregSystem, IStregSystemUI stregSystemUI)
        {
            this.stregSystem = stregSystem;
            this.ui = stregSystemUI;

            ui.CommandEntered += e => ParseCommand(e.Command);
        }

        private void ParseCommand(string command)
        {
            if (command.StartsWith(":"))
                ParseAdminCommand(command);
            else
            {
                string[] commandArray = command.Split(' ');

                if (commandArray.Length == 1)
                {
                    ParseUserInformation(commandArray[0]);
                } 
                else if (commandArray.Length == 2)
                {
                    ParseUserBuy(commandArray[0], commandArray[1]);
                }
                else if (commandArray.Length == 3)
                {
                    ParseUserMultiBuy(commandArray[0], commandArray[1], commandArray[2]);
                }
                else
                {
                    //TODO: Format error
                }
            }
        }

        private void ParseAdminCommand(string command)
        {
            //TODO: Lav det her :)
        }

        private void ParseUserInformation(string username)
        {
            if (ValidateUser(username))
                ui.DisplayUserInfo(stregSystem.GetUserByUsername(username));
        }

        private void ParseUserBuy(string username, string stringProductID)
        {
            if (!int.TryParse(stringProductID, out var productID))
            {
                ui.DisplayGeneralError($"Product ID is not in the correct format.");
                return;
            }

            if (ValidateUser(username) && ValidateProduct(productID))
            {
                BuyTransaction transaction = stregSystem.BuyProduct(stregSystem.GetUserByUsername(username), stregSystem.GetProductByID(productID));
                if (transaction != null)
                    ui.DisplayUserBuysProduct(transaction);
                else 
                    ui.DisplayInsufficientCash(stregSystem.GetUserByUsername(username), stregSystem.GetProductByID(productID));
            }
        }

        private void ParseUserMultiBuy(string username, string stringAmount, string stringProductID)
        {
            if (!int.TryParse(stringAmount, out var amount))
            {
                ui.DisplayGeneralError($"Amount is not in the correct format.");
                return;
            }
            if (!int.TryParse(stringProductID, out var productID))
            {
                ui.DisplayGeneralError($"Product ID is not in the correct format.");
                return;
            }

            //TODO: Find bedre måde at gøre det her på
            if (ValidateUser(username) && ValidateProduct(productID))
            {
                for (int i = 0; i < amount; i++)
                {
                    BuyTransaction transaction = stregSystem.BuyProduct(stregSystem.GetUserByUsername(username), stregSystem.GetProductByID(productID));
                    if (transaction != null)
                        ui.DisplayUserBuysProduct(transaction);
                    else
                        ui.DisplayInsufficientCash(stregSystem.GetUserByUsername(username), stregSystem.GetProductByID(productID));
                }
                
            }
        }

        /// <summary>
        /// Validate that a user with this username exists
        /// </summary>
        /// <param name="username">The username to check for</param>
        /// <returns>Returns true if the user exists, otherwise returns false</returns>
        private bool ValidateUser(string username)
        {
            try
            {
                stregSystem.GetUserByUsername(username);
                return true;
            }
            catch (NoneExistingItemException e)
            {
                ui.DisplayUserNotFound(username);
                ui.DisplayGeneralError(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Validate that a product with this ID exists
        /// </summary>
        /// <param name="productID">The ID to check for</param>
        /// <returns>Returns true if the product exists, otherwise returns false</returns>
        private bool ValidateProduct(int productID)
        {
            try
            {
                stregSystem.GetProductByID(productID);
                return true;
            }
            catch (NoneExistingItemException e)
            {
                ui.DisplayProductNotFound($"{productID}");
                ui.DisplayGeneralError(e.Message);
                return false;
            }
        }
    }
}
