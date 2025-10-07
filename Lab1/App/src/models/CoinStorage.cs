namespace Lab1.Models
{
    public class CoinStorage
    {
        private int[] denominations;
        private int[] counts;
        private int size;

        public CoinStorage()
        {
            denominations = new int[] { 1, 2, 5, 10, 20, 50, 100 };
            size = denominations.Length;
            counts = new int[size];
            
            for (int i = 0; i < size; i++)
            {
                int d = denominations[i];
                if (d == 1) counts[i] = 50;
                else if (d == 2) counts[i] = 40;
                else if (d == 5) counts[i] = 30;
                else if (d == 10) counts[i] = 25;
                else if (d == 20) counts[i] = 20;
                else if (d == 50) counts[i] = 15;
                else if (d == 100) counts[i] = 10;
                else counts[i] = 0;
            }
        }

        public CoinStorage(bool empty)
        {
            denominations = new int[] { 1, 2, 5, 10, 20, 50, 100 };
            size = denominations.Length;
            counts = new int[size];
            
            if (!empty)
            {
                for (int i = 0; i < size; i++)
                {
                    int d = denominations[i];
                    if (d == 1) counts[i] = 50;
                    else if (d == 2) counts[i] = 40;
                    else if (d == 5) counts[i] = 30;
                    else if (d == 10) counts[i] = 25;
                    else if (d == 20) counts[i] = 20;
                    else if (d == 50) counts[i] = 15;
                    else if (d == 100) counts[i] = 10;
                    else counts[i] = 0;
                }
            }
        }

        public bool IsValidCoin(int coin)
        {
            for (int i = 0; i < size; i++)
            {
                if (denominations[i] == coin)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddCoin(int coin, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным", nameof(quantity));
            
            for (int i = 0; i < size; i++)
            {
                if (denominations[i] == coin)
                {
                    counts[i] += quantity;
                    return;
                }
            }
            
            throw new ArgumentException("Недопустимый номинал: " + coin, nameof(coin));
        }

        public bool RemoveCoin(int coin, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным", nameof(quantity));
            
            for (int i = 0; i < size; i++)
            {
                if (denominations[i] == coin)
                {
                    if (counts[i] >= quantity)
                    {
                        counts[i] -= quantity;
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public int GetCoinCount(int coin)
        {
            for (int i = 0; i < size; i++)
            {
                if (denominations[i] == coin)
                {
                    return counts[i];
                }
            }
            return 0;
        }

        public int[] GetAllDenominations()
        {
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = denominations[i];
            }
            return result;
        }

        public int[] GetSortedDenoms()
        {
            int[] sorted = new int[size];
            for (int i = 0; i < size; i++)
            {
                sorted[i] = denominations[i];
            }
            Array.Sort(sorted, (a, b) => b.CompareTo(a));
            return sorted;
        }
    }
}
