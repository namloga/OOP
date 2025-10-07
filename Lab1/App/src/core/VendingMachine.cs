using System;
using System.Collections.Generic;
using System.Threading;
using Lab1.Models;

namespace Lab1.Core
{
    // Main vending machine class
    public class VendingMachine
    {
        private List<Product> products;   
        private int insertedMoney;  
        private int collectedMoney; 
        private CoinStorage cashbox; 

        public VendingMachine()
        {
            products = new List<Product>();
            insertedMoney = 0;
            collectedMoney = 0;
            cashbox = new CoinStorage();
            InitializeProducts();
        }

        private void InitializeProducts()
        {   // Drinks items
            products.Add(new Drink(1, "Кока-Кола", 120, 10));
            products.Add(new Drink(2, "Пепси", 120, 8));
            products.Add(new Drink(3, "Фанта", 150, 12));
            products.Add(new Drink(7, "Кофе", 80, 50));
            products.Add(new Drink(8, "Вода", 50, 30));
            // Food items
            products.Add(new Food(4, "Lays", 180, 15));
            products.Add(new Food(5, "Шоколад", 190, 6));
            products.Add(new Food(6, "Печенье", 150, 20));
            products.Add(new Food(9, "Мини-круассаны", 70, 18));
            products.Add(new Food(10, "Oreo", 120, 20));
        }

        public void ShowProducts()
        {
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("                ТОРГОВЫЙ АВТОМАТ ITMO");
            Console.WriteLine("═══════════════════════════════════════════════════");

            // Display products sorted by Id
            List<Product> productsSorted = new List<Product>(products);
            productsSorted.Sort((a, b) => a.Id.CompareTo(b.Id));
            for (int i = 0; i < productsSorted.Count; i++)
            {
                Product product = productsSorted[i];
                Console.WriteLine(product.ToString());
            }

            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("Внесено денег: " + insertedMoney.ToString() + " руб.");
            Console.WriteLine("═══════════════════════════════════════════════════");
        }


        // Main customer interaction flow
        public void ProcessCustomerInteraction()
        {
            Console.WriteLine();

            // Check if customer has money inserted
            if (insertedMoney <= 0)
            {
                Console.WriteLine("═══════════════════════════════════════════════════");
                Console.WriteLine("ПОЖАЛУЙСТА, ВНЕСИТЕ ДЕНЬГИ ПЕРЕД ПОКУПКОЙ");
                Console.WriteLine("Доступные номиналы: 1, 2, 5, 10, 20, 50, 100 руб.");
                Console.WriteLine("═══════════════════════════════════════════════════");
                Console.Write("Внесите сумму: ");

                string input = Console.ReadLine() ?? "0";
                int.TryParse(input, out int amount);

                if (amount == 0) return;

                if (cashbox.IsValidCoin(amount))
                {
                    Console.Write("Обрабатываем...");
                    Thread.Sleep(1000);
                    Console.WriteLine(" ОК");

                    insertedMoney += amount;
                    cashbox.AddCoin(amount, 1);
                    Console.WriteLine("Принято: " + amount.ToString() + " руб. Общая сумма: " + insertedMoney.ToString() + " руб.");
                    Console.WriteLine("Теперь вы можете выбрать товар!");
                }
                else
                {
                    Console.WriteLine("Недопустимый номинал! Доступны: 1, 2, 5, 10, 20, 50, 100 руб.");
                }
            }
            else
            {
                // Has money - allow product selection
                Console.WriteLine("═══════════════════════════════════════════════════");
                Console.WriteLine("ВЫБЕРИТЕ НОМЕР ТОВАРА");
                Console.WriteLine("999 - Вернуть деньги");
                Console.WriteLine("═══════════════════════════════════════════════════");
                Console.Write("Введите номер: ");

                string input = Console.ReadLine() ?? "0";

                if (input == "999")
                {
                    // Return money
                    Refund();
                    return;
                }

                int.TryParse(input, out int productId);

                // Validate product
                if (productId < 1 || !ContainsId(productId))
                {
                    Console.WriteLine("Неверный номер товара!");
                    return;
                }

                Product product = GetById(productId);

                if (product.Quantity <= 0)
                {
                    Console.WriteLine("Товар закончился!");
                    return;
                }

                // Check if enough money
                if (insertedMoney < product.Price)
                {
                    int needed = product.Price - insertedMoney;
                    Console.WriteLine();
                    Console.WriteLine("ВЫБРАННЫЙ ТОВАР: " + product.Name);
                    Console.WriteLine("Цена: " + product.Price.ToString() + " руб.");
                    Console.WriteLine("У вас: " + insertedMoney.ToString() + " руб.");
                    Console.WriteLine("Нужно ещё: " + needed.ToString() + " руб.");
                    Console.WriteLine();

                    // Show options menu
                    bool optionSelected = false;
                    while (!optionSelected)
                    {
                        Console.WriteLine("═══════════════════════════════════════════════════");
                        Console.WriteLine("КАК ПОСТУПИТЬ?");
                        Console.WriteLine("1. Больше денег");
                        Console.WriteLine("2. Выбрать другой товар");
                        Console.WriteLine("3. Вернуть все деньги");
                        Console.WriteLine("═══════════════════════════════════════════════════");
                        Console.Write("Выберите действие: ");

                        string optionChoice = Console.ReadLine() ?? "0";

                        switch (optionChoice)
                        {
                            case "1":
                                // Add more money
                                Console.WriteLine();
                                Console.WriteLine("ДОПОЛНИТЕЛЬНАЯ ОПЛАТА");
                                Console.WriteLine("Доступные номиналы: 1, 2, 5, 10, 20, 50, 100 руб.");
                                Console.Write("Внесите сумму: ");

                                string moneyInput = Console.ReadLine() ?? "0";
                                int.TryParse(moneyInput, out int amount);

                                if (amount > 0 && cashbox.IsValidCoin(amount))
                                {
                                    Console.Write("Обрабатываем...");
                                    Thread.Sleep(1000);
                                    Console.WriteLine(" ОК");

                                    insertedMoney += amount;
                                    cashbox.AddCoin(amount, 1);
                                    Console.WriteLine("Принято: " + amount.ToString() + " руб. Общая сумма: " + insertedMoney.ToString() + " руб.");

                                    if (insertedMoney >= product.Price)
                                    {
                                        Console.WriteLine("Отлично! Теперь достаточно денег!");
                                        optionSelected = true;
                                    }
                                    else
                                    {
                                        int stillNeeded = product.Price - insertedMoney;
                                        Console.WriteLine("Нужно ещё: " + stillNeeded.ToString() + " руб.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Недопустимый номинал!");
                                }
                                Console.WriteLine();
                                break;

                            case "2":
                                // Choose another product
                                Console.WriteLine("Выбираем другой товар...");
                                optionSelected = true;
                                return; 

                            case "3":
                                // Return all money
                                Console.WriteLine("Возвращаем все деньги...");
                                Refund();
                                optionSelected = true;
                                return;

                            default:
                                Console.WriteLine("Некорректный выбор! Попробуйте ещё раз.");
                                break;
                        }
                    }
                }

                // Now we have enough money - process purchase
                int change = insertedMoney - product.Price;

                if (change > 0 && !CanMakeChange(change))
                {
                    Console.WriteLine("Недостаточно монет для полной сдачи. Возвращаем внесенные деньги.");
                    Refund();
                    return;
                }

                // Complete purchase
                Console.WriteLine();
                Console.Write("Обрабатываем заказ...");
                Thread.Sleep(1000);
                Console.WriteLine("Готово!");

                insertedMoney -= product.Price;
                product.Quantity--;
                collectedMoney += product.Price;

                Console.WriteLine();
                Console.WriteLine("ПОКУПКА УСПЕШНА!");
                Console.WriteLine("Куплен: " + product.Name);
                Console.WriteLine("Потрачено: " + product.Price.ToString() + " руб.");

                if (change > 0)
                {
                    Console.WriteLine("Сдача: " + change.ToString() + " руб.");
                    DispenseChange(change);
                }

                insertedMoney = 0; // Reset for next customer
                Console.WriteLine("Спасибо за покупку!");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                try { Console.Clear(); } catch { }
            }
        }

        // Initial money deposit at session start
        public void InitialDeposit()
        {
            Console.WriteLine();
            Console.WriteLine("ПЕРВОНАЧАЛЬНОЕ ВНЕСЕНИЕ ДЕНЕГ");
            AddMoney();

            while (true)
            {
                Console.Write("Добавить ещё? (y/n): ");
                string more = (Console.ReadLine() ?? "").Trim().ToLower();

                if (more == "y" || more == "yes" || more == "д")
                {
                    AddMoney();
                }
                else if (more == "n" || more == "no" || more == "н" || more == "")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Введите y/n.");
                }
            }
        }

        public void AddMoney()
        {
            Console.WriteLine();
            Console.WriteLine("ВНЕСЕНИЕ ДЕНЕГ");
            Console.WriteLine("Доступные номиналы: 1, 2, 5, 10, 20, 50, 100 руб.");
            Console.Write("Введите сумму для внесения (или 0 для возврата в меню): ");

            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int amount);

            if (amount == 0) return;

            if (cashbox.IsValidCoin(amount))
            {
                Console.Write("Обрабатываем...");
                Thread.Sleep(1000);
                Console.WriteLine(" ОК");

                insertedMoney += amount;
                cashbox.AddCoin(amount, 1);
                Console.WriteLine("Внесено: " + amount.ToString() + " руб. Общая сумма: " + insertedMoney.ToString() + " руб.");
            }
            else
            {
                Console.WriteLine("Недопустимый номинал! Доступны: 1, 2, 5, 10, 20, 50, 100 руб.");
            }
        }

        public void Refund()
        {
            if (insertedMoney > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Возвращаем: " + insertedMoney.ToString() + " руб.");

                DispenseChange(insertedMoney);
                insertedMoney = 0;
            }
            else
            {
                Console.WriteLine("Нет денег для возврата.");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            try { Console.Clear(); } catch { } 
        }

        // Check if we can make exact change
        private bool CanMakeChange(int amount)
        {
            CoinStorage tempCashbox = new CoinStorage(true); 
            int[] denominations = cashbox.GetAllDenominations();

            for (int i = 0; i < denominations.Length; i++)
            {
                int count = cashbox.GetCoinCount(denominations[i]);
                tempCashbox.AddCoin(denominations[i], count);
            }

            int remaining = amount;
            int[] sortedDenoms = tempCashbox.GetSortedDenoms();

            for (int i = 0; i < sortedDenoms.Length; i++)
            {
                int coin = sortedDenoms[i];
                int coinsNeeded = remaining / coin;
                int coinsAvailable = tempCashbox.GetCoinCount(coin);
                int coinsToUse = Math.Min(coinsNeeded, coinsAvailable);

                if (coinsToUse > 0)
                {
                    remaining -= coinsToUse * coin;
                    tempCashbox.RemoveCoin(coin, coinsToUse); 
                }

                if (remaining <= 0) break;
            }

            return remaining <= 0;
        }

        private void DispenseChange(int amount)
        {
            Console.Write("Подготавливаем сдачу...");
            Thread.Sleep(800); 
            Console.WriteLine(" Готово!");

            Console.WriteLine("Сдача:");
            MoneyChange result = CalculateChange(amount);
            result.DisplayChange();
        }

        private MoneyChange CalculateChange(int amount)
        {
            MoneyChange result = new MoneyChange();
            int remaining = amount;
            int[] sortedDenoms = cashbox.GetSortedDenoms();

            for (int i = 0; i < sortedDenoms.Length; i++)
            {
                int coin = sortedDenoms[i];
                int coinsNeeded = remaining / coin;
                int coinsAvailable = cashbox.GetCoinCount(coin);
                int coinsToDispense = Math.Min(coinsNeeded, coinsAvailable);

                if (coinsToDispense > 0)
                {
                    remaining -= coinsToDispense * coin;
                    cashbox.RemoveCoin(coin, coinsToDispense);
                    result.AddChange(coin, coinsToDispense);
                }

                if (remaining <= 0) break;
            }

            return result;
        }

        public void AdminMode()
        {
            Console.Write("Введите пароль администратора: ");
            string password = Console.ReadLine() ?? "";

            if (password != "admin123")
            {
                Console.WriteLine("Неверный пароль!");
                return;
            }

            bool adminModeRunning = true;
            while (adminModeRunning)
            {
                try { Console.Clear(); } catch { }
                ShowProducts();

                Console.WriteLine();
                Console.WriteLine("╔═══════════════════════════════════════╗");
                Console.WriteLine("║          РЕЖИМ АДМИНИСТРАТОРА         ║");
                Console.WriteLine("╠═══════════════════════════════════════╣");
                Console.WriteLine("║ 1. Пополнить товары                   ║");
                Console.WriteLine("║ 2. Собрать деньги                     ║");
                Console.WriteLine("║ 0. Выход из админ-режима              ║");
                Console.WriteLine("╚═══════════════════════════════════════╝");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine() ?? "0";

                switch (choice)
                {
                    case "1":
                        RestockProducts();
                        break;
                    case "2":
                        CollectMoney();
                        break;
                    case "0":
                        adminModeRunning = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                if (adminModeRunning)
                {
                    Console.WriteLine();
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear(); 
                }
            }
        }

        private void RestockProducts()
        {
            Console.WriteLine();
            Console.WriteLine("ПОПОЛНЕНИЕ ТОВАРОВ");
            ShowProducts();

            Console.Write("Введите номер товара для пополнения (ID): ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int productId);

            if (productId < 1 || !ContainsId(productId))
            {
                Console.WriteLine("Неверный номер товара!");
                return;
            }

            Product product = GetById(productId);
            Console.WriteLine("Текущий остаток " + product.Name + ": " + product.Quantity + " шт.");
            Console.Write("Добавить количество: ");

            string quantityInput = Console.ReadLine() ?? "0";
            int.TryParse(quantityInput, out int quantity);

            if (quantity > 0)
            {
                product.Quantity += quantity;
                Console.WriteLine("Добавлено " + quantity + " шт. Новый остаток: " + product.Quantity + " шт.");
            }
            else
            {
                Console.WriteLine("Неверное количество!");
            }
        }

        private Product GetById(int id)
        {
            var p = products.Find(x => x.Id == id);
            if (p is null)
                throw new InvalidOperationException($"Товар с ID={id} не найден.");
            return p;
        }


        private bool ContainsId(int id)
        {
            return products.Exists(p => p.Id == id);
        }

        private void CollectMoney()
        {
            Console.WriteLine();
            Console.WriteLine("Собираем деньги из автомата...");
            int target = collectedMoney;

            if (target <= 0)
            {
                Console.WriteLine("Нет средств для сбора.");
                return;
            }

            int remaining = target;
            int[] denomsDesc = cashbox.GetSortedDenoms();

            var moneyMachine = new Dictionary<int, int>();

            foreach (int coin in denomsDesc)
            {
                int available = cashbox.GetCoinCount(coin);
                int need = remaining / coin;
                int take = Math.Min(available, need);
                if (take > 0)
                {
                    moneyMachine[coin] = take;
                    remaining -= take * coin;
                    if (remaining == 0) break;
                }
            }

            int totalAmount = target - remaining;

            if (totalAmount > 0)
            {
                foreach (var tmp in moneyMachine)
                {
                    cashbox.RemoveCoin(tmp.Key, tmp.Value);
                }

                collectedMoney -= totalAmount;

                if (remaining > 0)
                {
                    Console.WriteLine("Снято частично: " + totalAmount.ToString() + " из " + target.ToString() + " руб.");
                }
                else
                {
                    Console.WriteLine("Снято: " + totalAmount.ToString() + " руб.");
                }
            }
            else
            {
                Console.WriteLine("Не удалось снять средства из-за нехватки подходящих монет.");
            }
            
        }
    }
}
