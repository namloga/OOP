using System;
using Lab1.Core;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
         {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            VendingMachine machine = new VendingMachine();
            bool running = true;

            while (running)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("╔═══════════════════════════════════════╗");
                    Console.WriteLine("║        ТОРГОВЫЙ АВТОМАТ ITMO          ║");
                    Console.WriteLine("╠═══════════════════════════════════════╣");
                    Console.WriteLine("║ 1. Режим покупателя                   ║");
                    Console.WriteLine("║ 2. Режим администратора               ║");
                    Console.WriteLine("║ 0. Выход                              ║");
                    Console.WriteLine("╚═══════════════════════════════════════╝");
                    Console.Write("Выберите режим: ");

                    string choice = Console.ReadLine() ?? "0";

                    switch (choice)
                    {
                        case "1":
                            CustomerMode(machine);
                            break;
                        case "2":
                            machine.AdminMode();
                            break;
                        case "0":
                            Console.WriteLine();
                            Console.WriteLine("Спасибо за использование торгового автомата!");
                            Console.WriteLine("До свидания!");
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор! Попробуйте ещё раз.");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        // Customer mode - shopping and money deposit
        static void CustomerMode(VendingMachine machine)
        {
            bool customerRunning = true;

            // Allow multiple money deposits before shopping
            machine.InitialDeposit();

            while (customerRunning)
            {
                try
                {
                    Console.Clear();

                    machine.ShowProducts();
                    machine.ProcessCustomerInteraction();

                    Console.WriteLine();
                    Console.WriteLine("═══════════════════════════════════════════════════");
                    Console.WriteLine("1. Продолжить покупки");
                    Console.WriteLine("0. Вернуться в главное меню");
                    Console.WriteLine("═══════════════════════════════════════════════════");
                    Console.Write("Выберите действие: ");

                    string continueChoice = Console.ReadLine() ?? "0";

                    switch (continueChoice)
                    {
                        case "1":
                            Console.Clear();
                            break;
                        case "0":
                            customerRunning = false;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор! Попробуйте ещё раз.");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            Console.Clear(); 
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }
    }
}