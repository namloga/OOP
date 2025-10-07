using System;

namespace Lab1.Models
{
    public class Food : Product
    {
        public Food(int id, string name, int price, int quantity)
            : base(id, name, price, quantity) 
        {
        }

        public override string ToString()
        {
            string idStr = Id.ToString("D2");
            return idStr + ". " + Name + " (Закуска) - " + Price.ToString() + " руб. (Остаток: " + Quantity + " шт.)";
        }

    }
}
