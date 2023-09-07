using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MoneyTracking
{
    /*
     * user account. Here all data is stored. All help methods for data manipulation and display also reside here
     */
    [Serializable]
    internal class Account
    {
        List<Item> items = new List<Item>();
        decimal balance = 0;
        String filePath = ".\\account.tsv";
        String separator = ";";

        public Account() { }
    
    
    
        /*
         * Adds an item to the item list
         */
        public void addItem(Item item)
        { 
            items.Add(item);
            setBalance();
        }

        /*
         * Removes an item from the item list
         */
        public void removeItem(Item item)
        {
            items.Remove(item);
            setBalance();
        }

        /*
        * Removes an item from the item list by index
        */
        public void removeItemByIndex(int index)
        {
            items.RemoveAt(index);
            setBalance();
        }


        //TODO MODIFY ITEM??????
        protected void modifyItem(Item item)
        {
            int index = items.IndexOf(item);
            this.items.ElementAt(index);
            setBalance();
        }

        /*
         * return only income items
         */
        public List<Item> getIncomes()
        {
            return this.items.Where(item => item.Type == TYPE.INCOME).ToList();         
        }

        /*
         * return only expense items
         */
        public List<Item> getExpenses()
        {
            return this.items.Where(item => item.Type == TYPE.EXPENSE).ToList();
        }

        public decimal getBalance() { return this.balance; }

        public List<Item> getItems() { return items; }

        public Item getItemByIndex(int index)
        {
            return items.ElementAt(index);
        }

        /*
         * Calculates the account balance
         */
        protected void setBalance()
        {
            List<Item> incomes = getIncomes();
            List<Item> expenses = getExpenses();

            this.balance = incomes.Sum(item => item.Amount) - expenses.Sum(item => item.Amount);
        }

        protected void setItems(List<Item> items) { this.items = items; }

        public void setItemByIndex(int index, Item item)
        {    
            items.Insert(index, item);
            items.RemoveAt(index + 1);
        }

        /*
         * Saves a binary account object to file
         */
        public void saveAccount()
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                foreach (var item in this.items)
                {
                    sw.WriteLine(item.Type + separator + item.Title + separator + item.Date+ separator + item.Amount);
                }
            }
        }

        /*
         * Loads a binary account object from file
         */
        public void loadAccount()
        {
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (var sr = new StreamReader(fs))
            {
                String line;
                while ((line = sr.ReadLine()) !=null )
                {
                    String[] itemString = line.Split(separator);
                    if (itemString[0].Trim() == "EXPENSE")
                    {
                        addItem(new Expense(itemString[1].Trim(), StringToDate(itemString[2].Trim()), decimal.Parse(itemString[3].Trim())));
                    }else if (itemString[0].Trim() == "INCOME")
                    {
                        addItem(new Income(itemString[1].Trim(), StringToDate(itemString[2].Trim()), decimal.Parse(itemString[3].Trim())));
                    }
                }
            }
        }

        /*
         * Converts String date to date object
         */
        public static DateTime StringToDate(string theDateInStringFormat)
        {
            DateTime result;
            if (DateTime.TryParse(theDateInStringFormat, out result))
            {
                return result;
            }

            return new DateTime();
        }
    }
}
