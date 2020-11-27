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

        /// <summary>
        /// Command parser, used for parse the input command and choose the correct parsing method.
        /// </summary>
        /// <param name="command">Command to parse</param>
        private void ParseCommand(string command)
        {
            if (command.StartsWith(":"))
                try
                {
                    ParseAdminCommand(command);
                }
                catch (NoneExistingUserException e)
                {
                    ui.DisplayUserNotFound(e.Message);
                }
                catch (NoneExistingProductException e)
                {
                    ui.DisplayProductNotFound(e.Message);
                }
            else
            {
                string[] commandArray = command.Split(' ');

                if (commandArray.Length == 1)
                {
                    try
                    {
                        ParseUserInformation(commandArray[0]);
                    }
                    catch (NoneExistingUserException e)
                    {
                        ui.DisplayUserNotFound(e.Message);
                    }

                } 
                else if (commandArray.Length == 2)
                {
                    try
                    {
                        ParseUserBuy(commandArray[0], commandArray[1]);
                    }
                    catch (NoneExistingUserException e)
                    {
                        ui.DisplayUserNotFound(e.Message);
                    }
                    catch (NoneExistingProductException e)
                    {
                        ui.DisplayProductNotFound(e.Message);
                    }
                    catch (InsufficientCreditsException)
                    {
                        ui.DisplayInsufficientCash(stregSystem.GetUserByUsername(commandArray[0]), stregSystem.GetProductByID(Convert.ToInt32(commandArray[1])));
                    }
                    
                }
                else if (commandArray.Length == 3)
                {
                    try
                    {
                        ParseUserMultiBuy(commandArray[0], commandArray[1], commandArray[2]);
                    }
                    catch (NoneExistingUserException e)
                    {
                        ui.DisplayUserNotFound(e.Message);
                    }
                    catch (NoneExistingProductException e)
                    {
                        ui.DisplayProductNotFound(e.Message);
                    }
                    catch (InsufficientCreditsException)
                    {
                        ui.DisplayInsufficientCash(stregSystem.GetUserByUsername(commandArray[0]), stregSystem.GetProductByID(Convert.ToInt32(commandArray[1])));
                    }
                }
                else
                { 
                    ui.DisplayTooManyArgumentsError(command);
                }
            }
        }

        /// <summary>
        /// Parse as an admin command.
        /// </summary>
        /// <param name="command">Command to parse</param>
        private void ParseAdminCommand(string command)
        {
            string[] commandArray = command.Split(' ');
            if (adminCommands.ContainsKey(commandArray[0]))
                adminCommands[commandArray[0]].Invoke(commandArray);
            else 
                ui.DisplayAdminCommandNotFoundMessage(commandArray[0]);
        }

        /// <summary>
        /// Parse as user information command.
        /// </summary>
        /// <param name="username">Username to check information about</param>
        private void ParseUserInformation(string username)
        {
            ui.DisplayUserInfo(stregSystem.GetUserByUsername(username));
        }

        /// <summary>
        /// Parse as a buy command.
        /// </summary>
        /// <param name="username">Username of buyer</param>
        /// <param name="stringProductID">Product ID of product to buy</param>
        private void ParseUserBuy(string username, string stringProductID)
        {
            if (stringProductID == "")
            {
                ParseUserInformation(username);
                return;
            }
            if (!int.TryParse(stringProductID, out var productID))
            {
                ui.DisplayGeneralError($"Product ID is not in the correct format.");
                return;
            }

            Product product = stregSystem.GetProductByID(productID);
            User user = stregSystem.GetUserByUsername(username);

            if (product.Active)
                ui.DisplayUserBuysProduct(stregSystem.BuyProduct(user, product));
            else
                ui.DisplayProductNotFound($"{productID}");
        }

        /// <summary>
        /// Parse as a multibuy command.
        /// </summary>
        /// <param name="username">Username of buyer</param>
        /// <param name="stringAmount">Amount to buy</param>
        /// <param name="stringProductID">Product ID of product to buy</param>
        private void ParseUserMultiBuy(string username, string stringAmount, string stringProductID)
        {
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

            Product product = stregSystem.GetProductByID(productID);
            User user = stregSystem.GetUserByUsername(username);

            if (!product.Active)
            {
                ui.DisplayProductNotFound($"{productID}");
                return;
            }
            if ((user.Balance >= product.Price * amount) || product.CanBeBoughtOnCredit)
            {
                BuyTransaction transaction = null;
                for (int i = 0; i < amount; i++)
                {
                    transaction = stregSystem.BuyProduct(user, product);
                }
                ui.DisplayUserBuysProduct(amount, transaction);
            }
            else 
                ui.DisplayInsufficientCash(user, amount, product);
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
                    stregSystem.GetProductByID(Convert.ToInt32(args[1])).Active = true;
                    stregSystem.UpdateActiveProductList();
                    ui.Start();
                }
                else if(args.Length < 2)
                    ui.DisplayGeneralError("Not enough arguments");
                else
                    ui.DisplayTooManyArgumentsError(args[0]);
            });
            adminCommands.Add(":deactivate", (args) =>
            {
                if (args.Length == 2)
                {
                    stregSystem.GetProductByID(Convert.ToInt32(args[1])).Active = false;
                    stregSystem.UpdateActiveProductList();
                    ui.Start();
                }
                else if (args.Length < 2)
                    ui.DisplayGeneralError("Not enough arguments");
                else
                    ui.DisplayTooManyArgumentsError(args[0]);
            });
            adminCommands.Add(":crediton", (args) =>
            {
                if (args.Length == 2)
                    stregSystem.GetProductByID(Convert.ToInt32(args[1])).CanBeBoughtOnCredit = true;
                else if (args.Length < 2)
                    ui.DisplayGeneralError("Not enough arguments");
                else
                    ui.DisplayTooManyArgumentsError(args[0]);
            });
            adminCommands.Add(":creditoff", (args) =>
            {
                if (args.Length == 2)
                    stregSystem.GetProductByID(Convert.ToInt32(args[1])).CanBeBoughtOnCredit = false;
                else if (args.Length < 2)
                    ui.DisplayGeneralError("Not enough arguments");
                else
                    ui.DisplayTooManyArgumentsError(args[0]);
            });
            adminCommands.Add(":addcredits", (args) =>
            {
                if (args.Length == 3)
                    stregSystem.GetUserByUsername(args[1]).AddBalance(Convert.ToDecimal(args[2]));
                else if (args.Length < 3) 
                    ui.DisplayGeneralError("Not enough arguments");
                else
                    ui.DisplayTooManyArgumentsError(args[0]);
            });
        }
    }
}
