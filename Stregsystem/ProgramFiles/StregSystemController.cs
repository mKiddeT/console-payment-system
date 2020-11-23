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

        private Dictionary<string, Action<string[]>> adminCommands;

        public StregSystemController(IStregSystem stregSystem, IStregSystemUI stregSystemUI)
        {
            this.stregSystem = stregSystem;
            this.ui = stregSystemUI;

            InstantiateAdminCommands();

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
            string[] commandArray = command.Split(' ');
            if (adminCommands.ContainsKey(commandArray[0]))
                adminCommands[commandArray[0]].Invoke(commandArray);
            else 
                ui.DisplayAdminCommandNotFoundMessage(commandArray[0]);
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
                ui.DisplayUserBuysProduct(UserBuyProduct(username, productID));
            }
        }

        private void ParseUserMultiBuy(string username, string stringAmount, string stringProductID)
        {
            //TODO: Check om den kan købe amount, inden den gør noget
            if (!int.TryParse(stringAmount, out var amount) || amount <= 0)
            {
                ui.DisplayGeneralError($"Amount is not in the correct format.");
                return;
            }
            if (!int.TryParse(stringProductID, out var productID))
            {
                ui.DisplayGeneralError($"Product ID is not in the correct format.");
                return;
            }

            if (ValidateUser(username) && ValidateProduct(productID))
            {
                if (stregSystem.GetUserByUsername(username).GetBalance() >= stregSystem.GetProductByID(productID).Price * amount)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        if (i == amount-1)
                            ui.DisplayUserBuysProduct(amount, UserBuyProduct(username, productID));
                        else
                            UserBuyProduct(username, productID);
                    }
                }
                else 
                    ui.DisplayInsufficientCash(stregSystem.GetUserByUsername(username), amount, stregSystem.GetProductByID(productID));
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
                return false;
            }
        }

        /// <summary>
        /// The functionality of a user trying to buy a product.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <param name="productID">ID of the product.</param>
        private BuyTransaction UserBuyProduct(string username, int productID)
        {
            try
            {
                BuyTransaction transaction = stregSystem.BuyProduct(stregSystem.GetUserByUsername(username), stregSystem.GetProductByID(productID));
                return transaction;
            }
            catch (NoneExistingItemException e)
            {
                ui.DisplayGeneralError(e.Message);
            }
            catch (InsufficientCreditsException)
            {
                ui.DisplayInsufficientCash(stregSystem.GetUserByUsername(username), stregSystem.GetProductByID(productID));
            }

            return null;
        }

        /// <summary>
        /// Instantiate known admin commands to dictionary.
        /// </summary>
        private void InstantiateAdminCommands()
        {
            adminCommands = new Dictionary<string, Action<string[]>>();

            adminCommands.Add(":quit", (args) =>
            {
                if (args.Length == 1)
                    ui.Close();
                else 
                    ui.DisplayTooManyArgumentsError(args[0]);
            });
            adminCommands.Add(":q", adminCommands[":quit"]);
            adminCommands.Add(":activate", (args) =>
            {
                if (args.Length == 2)
                {
                    try
                    {
                        stregSystem.GetProductByID(Convert.ToInt32(args[1])).Active = true;
                        stregSystem.UpdateActiveProductList();
                        ui.Start();
                    }
                    catch (FormatException e)
                    {
                        ui.DisplayGeneralError(e.Message);
                    }
                    catch (NoneExistingItemException)
                    {
                        ui.DisplayProductNotFound(args[1]);
                    }
                }
                else
                {
                    ui.DisplayTooManyArgumentsError(args[0]);
                }
            });
            adminCommands.Add(":deactivate", (args) =>
            {
                if (args.Length == 2)
                {
                    try
                    {
                        stregSystem.GetProductByID(Convert.ToInt32(args[1])).Active = false;
                        stregSystem.UpdateActiveProductList();
                        ui.Start();
                    }
                    catch (FormatException e)
                    {
                        ui.DisplayGeneralError(e.Message);
                    }
                    catch (NoneExistingItemException)
                    {
                        ui.DisplayProductNotFound(args[1]);
                    }
                }
                else
                {
                    ui.DisplayTooManyArgumentsError(args[0]);
                }
            });
            adminCommands.Add(":crediton", (args) =>
            {
                if (args.Length == 2)
                {
                    try
                    {
                        stregSystem.GetProductByID(Convert.ToInt32(args[1])).CanBeBoughtOnCredit = true;
                    }
                    catch (FormatException e)
                    {
                        ui.DisplayGeneralError(e.Message);
                    }
                    catch (NoneExistingItemException)
                    {
                        ui.DisplayProductNotFound(args[1]);
                    }
                } else
                {
                    ui.DisplayTooManyArgumentsError(args[0]);
                }
            });
            adminCommands.Add(":creditoff", (args) =>
            {
                if (args.Length == 2)
                {
                    try
                    {
                        stregSystem.GetProductByID(Convert.ToInt32(args[1])).CanBeBoughtOnCredit = false;
                    }
                    catch (FormatException e)
                    {
                        ui.DisplayGeneralError(e.Message);
                    }
                    catch (NoneExistingItemException)
                    {
                        ui.DisplayProductNotFound(args[1]);
                    }
                } else
                {
                    ui.DisplayTooManyArgumentsError(args[0]);
                }
            });
            adminCommands.Add(":addcredits", (args) =>
            {
                if (args.Length == 3)
                {
                    try
                    {
                        stregSystem.GetUserByUsername(args[1]).AddBalance(Convert.ToDecimal(args[2]));
                    }
                    catch (FormatException e)
                    {
                        ui.DisplayGeneralError(e.Message);
                    }
                    catch (NoneExistingItemException)
                    {
                        ui.DisplayUserNotFound(args[1]);
                    }
                } else
                {
                    ui.DisplayTooManyArgumentsError(args[0]);
                }
                
            });
        }
    }
}
