using System;

namespace Lab1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Product(int id, string name, int price, int quantity)
        {
            if (id <= 0)
                throw new ArgumentException("Номер товара должен быть положительным", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым", nameof(name));
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной", nameof(price));
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным", nameof(quantity));
            
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            string idStr = Id.ToString("D2");
            return idStr + ". " + Name + " - " + Price.ToString() + " руб. (Остаток: " + Quantity + " шт.)";
        }
    }
}
