using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracking
{
    internal class Expense : Item
    {
        public Expense(String title, DateTime month, Decimal amount) : base(title, month, amount) 
        {
            Type = TYPE.EXPENSE;
        }

        public void toString()
        {
            base.ToString();
        }
    }
}
