using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracking
{

    internal class Income : Item
    {
        public Income(String title, DateTime month, Decimal amount) : base(title, month, amount)
        {
            Type = TYPE.INCOME;
        }

        public void toString()
        {
            base.ToString();
        }

    }
}
