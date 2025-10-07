using System;

namespace Lab1.Models
{
    public class MoneyChange
    {
        private int[] denominations;
        private int[] quantities;
        private int count;

        public MoneyChange()
        {
            denominations = new int[10];
            quantities = new int[10];
            count = 0;
        }

        public void AddChange(int denomination, int quantity)
        {
            if (quantity > 0)
            {
                denominations[count] = denomination;
                quantities[count] = quantity;
                count++;
            }
        }

        public void DisplayChange()
        {
            if (count == 0)
            {
                Console.WriteLine("  0 руб.");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("  " + denominations[i].ToString() + " руб. × " + quantities[i]);
            }
        }

        public int GetDenomination(int index)
        {
            if (index >= 0 && index < count)
                return denominations[index];
            return 0;
        }

    }
}
