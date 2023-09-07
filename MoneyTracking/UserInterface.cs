using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracking
{
    internal class UserInterface
    {
        static int paddingSize = 25;
        String printoutDivider = "-----------------------------------------------------------------------------------------------------------------------";
        String printoutHeader = "#".PadRight(paddingSize) + "Title".PadRight(paddingSize) + "Month".PadRight(paddingSize) + "Amount".PadRight(paddingSize);
        private Account account = new Account();
        public UserInterface() { }
        private String[] menuOptions = {
            "Show Items",
            "Add New Item",
            "Edit Item",
            "Save and Exit"
        };


        /*
         * Main menu logic for user to make action regarding the account
         */
        public void mainLoop()
        {
            Boolean exit = false;

            account.loadAccount();
            printMsg("Welcome to TrackMoney");
            while (!exit)
            {
                printMsg("You have currently (" + account.getBalance() + ") kr on your account\n" +
                                    "Pick an option:");

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    printMsg("(", Color.WHITE, false);
                    printMsg("" + (i+1), Color.YELLOW, false);
                    printMsg(") " + this.menuOptions[i]);
                }

                String userIput = Console.ReadLine();

                switch (userIput)
                {
                    case "1":
                        showItems();
                        break;
                    case "2":
                        addItem();
                        break;
                    case "3":
                        editItem();
                        break;
                    case "4":
                        account.saveAccount();
                        exit = true;
                        break;
                }
            }

        }

        /*
         * Dislays Items. All, Expenses or Income 
         */
        private void showItems()
        {
            Boolean exit = false;

            while (!exit)
            {
                printMsg("Show (a)ll, (e)xpense, (i)ncome, (x)exit");
                String userInput = Console.ReadLine().Trim().ToLower();

                switch (userInput)
                {
                    case "a":
                        printList(account.getItems());
                        break;
                    case "e":
                        printList(account.getExpenses());
                        break;
                    case "i":
                        printList(account.getIncomes());
                        break;
                    case "x":
                        exit = true;
                        break;
                }
            }
        }

        /*
         * Adds item to the account. It can be Exopense or an income
         */
        private void addItem()
        {
            Boolean exitAddItem = false;

            while (!exitAddItem)
            {
                printList(account.getItems());
                printMsg("You have currently (" + account.getBalance() + ") kr on your account\n" +
                    "Pick an option:");

                String userInput = "";
                while (userInput != "e" && userInput != "i" && userInput != "x")
                {
                    printMsg("What type of Item you want to add? (e)xpense, (i)ncome, (x)exit");
                    userInput = Console.ReadLine().Trim().ToLower();
                }

                String title = "";
                DateTime date = new DateTime();
                Decimal amount = 0;

                if (userInput != "x")
                {
                    printMsg("What is the title?");
                    title = Console.ReadLine().Trim();

                    Boolean falseDate = true;
                    while (falseDate)
                    {
                        printMsg("Date: ");
                        DateTime tempDate = new DateTime();
                        if (DateTime.TryParse(Console.ReadLine().Trim(), out tempDate))
                        {
                            falseDate = false;
                            date = tempDate;
                        }
                        else
                        {
                            printMsg("Wrong date: yyyy-mm-dd", Color.RED);
                        }
                    }

                    Boolean falseAmount = true;
                    while (falseAmount)
                    {
                        printMsg("Amount: ");
                        String tempAmount = Console.ReadLine().Trim().ToLower();
                        decimal outAmount = 0;
                        if (decimal.TryParse(tempAmount, out outAmount))
                        {
                            falseAmount = false;
                            amount = outAmount;
                        }
                        else
                        {
                            printMsg("Wrong amount: £££,£££", Color.RED);
                        }
                    }

                    if (userInput == "e")
                    {
                        Item tempItem = new Expense(title, date, amount);
                        account.addItem(tempItem);
                        printMsg("Done.");
                    }
                    else if (userInput == "i")
                    {
                        Item tempItem = new Income(title, date, amount);
                        account.addItem(tempItem);
                        printMsg("Done.");
                    }
                }else
                {
                    exitAddItem = true;
                }
            }
        }

        private void editItem()
        {
            Boolean exitAddItem = false;

            while (!exitAddItem)
            {
                printList(account.getItems());
                printMsg("You have currently (" + account.getBalance() + ") kr on your account\n" +
                    "Pick an option:");

                String userInput = "";
                while (userInput != "e" && userInput != "r" && userInput != "x")
                {
                    printMsg("What action? (e)dit, (r)emove, (x)exit");
                    userInput = Console.ReadLine().Trim().ToLower();
                }


                
                if (userInput != "x")
                {

                    //Get item index to remove/edit
                    String itemNr = "";
                    int outIndex = 0;
                    printList(account.getItems());
                    Boolean falseNumber = true;
                    while(falseNumber)
                    {
                        printMsg("Which item #?");
                        itemNr = Console.ReadLine().Trim();
                        
                        if (int.TryParse(itemNr, out outIndex) && int.Parse(itemNr) >= 0 && int.Parse(itemNr) < account.getItems().Count)
                        {
                            falseNumber = false;
                        }
                        else
                        {
                            printMsg("Wrong index", Color.RED);
                        }
                    }

                    
                    //edit selected index
                    if (userInput == "e")
                    {
                        String userAction = "";
                        while (userAction != "y" && userAction != "t" && userAction != "d" && userAction != "a" && userAction != "x")
                        {
                            printMsg("What should be edited? (y)type, (t)itle, (d)ate, (a)mount, (x)exit");
                            userAction = Console.ReadLine().Trim().ToLower();
                        }

                        if (userAction != "x")
                        {
                            Item tempItem = account.getItemByIndex(outIndex);
                            switch (userAction)
                            {
                                case "y":
                                    String tempType = "";
                                    while (tempType != "e" && tempType != "i")
                                    {
                                        printMsg("New type: (e)xpense, (i)ncome, (x)exit");
                                        tempType = Console.ReadLine().Trim().ToLower();
                                    }
                                    if (tempType == "e")
                                    {
                                        account.setItemByIndex(outIndex, new Expense(tempItem.Title, tempItem.Date, tempItem.Amount));
                                    }
                                    else if (tempType == "i")
                                    {
                                        account.setItemByIndex(outIndex, new Income(tempItem.Title, tempItem.Date, tempItem.Amount));
                                    }
                                    break;
                                case "t":
                                    printMsg("New title: ");
                                    String tempTile = Console.ReadLine().Trim();
                                    tempItem.Title = tempTile;
                                    account.setItemByIndex(outIndex, tempItem);
                                    break;
                                case "d":
                                    Boolean falseDate= true;
                                    while (falseDate)
                                    {
                                        printMsg("New date: ");
                                        DateTime tempDate = new DateTime();
                                        if (DateTime.TryParse(Console.ReadLine().Trim(), out tempDate))
                                        {
                                            falseDate = false;
                                            tempItem.Date = tempDate;
                                        }
                                        else
                                        {
                                            printMsg("Wrong date: yyyy-mm-dd", Color.RED);
                                        }
                                    }
                                    account.setItemByIndex(outIndex, tempItem);
                                    break;
                                case "a":
                                    Boolean falseAmount = true;
                                    while (falseAmount)
                                    {
                                        printMsg("New amount: ");
                                        String tempAmount = Console.ReadLine().Trim().ToLower();
                                        decimal outAmount = 0;
                                        if (decimal.TryParse(tempAmount, out outAmount))
                                        {
                                            falseAmount = false;
                                            tempItem.Amount = outAmount;
                                        }
                                        else
                                        {
                                            printMsg("Wrong amount: £££,£££", Color.RED);
                                        }
                                    }    
                                    account.setItemByIndex(outIndex, tempItem);
                                    break;
                            }
                            

                            printMsg("Done.");
                        }


                    }
                    else if (userInput == "r")
                    {
                        account.removeItemByIndex(outIndex);
                        printMsg("Done.");
                    }
                }
                else
                {
                    exitAddItem = true;
                }
            }
        }

        private void removeItem(Item item)
        {
            account.removeItem(item);
        }
        private void updateItem() { }
        private void saveAccount() { }
        private void loadAccount() { }

        /*
         * Show the account details to the user
         */
        private void printList(List<Item> list)
        {
            printMsg(printoutDivider);
            printMsg(printoutHeader);
            printMsg(printoutDivider);
            for (int i = 0; i < list.Count; i++)
            {
                printMsg(i+"".PadRight(paddingSize) + list[i].ToString());
            }
            printMsg(printoutDivider);
            printMsg("");
        }

        /*
 * Changes console output foreground color to one of pre defined Enums. 
 * After that goes back to default white.
 * 
 */
        private void printMsg(String msg, Color color = Color.WHITE, bool newLine = true)
        {
            switch (color)
            {
                case Color.WHITE:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Color.GREEN:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Color.BLUE:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Color.YELLOW:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Color.CYAN:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case Color.RED:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            if (newLine)
            {
                Console.WriteLine(msg);
            }
            else
            {
                Console.Write(msg);
            }

            if (Console.ForegroundColor != ConsoleColor.White)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }



enum Color
{
    WHITE,
    BLUE,
    GREEN,
    YELLOW,
    CYAN,
    RED
}
    }
}
