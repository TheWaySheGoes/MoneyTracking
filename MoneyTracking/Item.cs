using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracking
{
    /*
     * Base class for items 
     */
    internal class Item
    {
        internal TYPE Type { get; set; }
        internal String Title { get; set; }
        internal DateTime Date { get; set; }
        internal Decimal Amount { get; set; }
        static int paddingSize = 25;


        public Item(String title, DateTime date, Decimal amount)
        {
            this.Title = title;
            this.Date = date;
            this.Amount = amount;
        }

        public string ToString()
        {
            return Title.PadRight(paddingSize) + Date.ToString("yyy-MM-dd").PadRight(paddingSize) + (Type == TYPE.EXPENSE ? "-" : "") + Amount.ToString().PadRight(paddingSize);
        }

    }

    /**
     * Defines type of the Item object AKA type
     * */
    public enum TYPE
    {
        EXPENSE,
        INCOME
    }
}
