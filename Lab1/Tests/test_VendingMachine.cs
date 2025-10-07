using System;
using System.IO;
using Xunit;
using Lab1.Core;

namespace Tests
{
    public class VendingMachine_Tests
    {
        // AddMoney - valid denomination
        [Fact]
        public void AddMoney_ValidCoin_PrintsResultOk()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;
            var s = new StringWriter();
            Console.SetIn(new StringReader("50\n"));
            Console.SetOut(s);

            // Act
            vm.AddMoney();

            // Assert
            Console.SetIn(prevIn);
            Console.SetOut(prevOut);
            var output = s.ToString();
            Assert.Contains("ОК", output);
            Assert.Contains("Внесено:", output);
        }

        // AddMoney - invalid denomination
        [Fact]
        public void AddMoney_InvalidCoin_ResultWarning()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;
            var s = new StringWriter();
            Console.SetIn(new StringReader("3\n")); 
            Console.SetOut(s);

            // Act
            vm.AddMoney();

            // Assert
            Console.SetIn(prevIn);
            Console.SetOut(prevOut);
            Assert.Contains("Недопустимый номинал", s.ToString());
        }

        // Purchase successful no change required
        [Fact]
        public void Buy_Succeeds_NoChange()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;

            Console.SetIn(new StringReader("50\n"));
            Console.SetOut(new StringWriter());
            vm.AddMoney();

            Console.SetIn(new StringReader("20\n"));
            Console.SetOut(new StringWriter());
            vm.AddMoney();

            Console.SetIn(new StringReader("10\n"));
            Console.SetOut(new StringWriter());
            vm.AddMoney();

            // Act
            var sBuy = new StringWriter();
            Console.SetIn(new StringReader("7\n")); 
            Console.SetOut(sBuy);
            try { vm.ProcessCustomerInteraction(); } catch (InvalidOperationException) { } 
            Console.SetIn(prevIn);
            Console.SetOut(prevOut);

            // Assert
            var output = sBuy.ToString();
            Assert.Contains("ПОКУПКА УСПЕШНА", output);
            Assert.Contains("Куплен: Кофе", output);
        }

        // Buy when you don't have enough money
        [Fact]
        public void Buy_NotEnoughMoney_ResultNeededAmount()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;

            Console.SetIn(new StringReader("50\n"));
            Console.SetOut(new StringWriter());
            vm.AddMoney();

            // Act
            var s = new StringWriter();

            Console.SetIn(new StringReader("1\n2\n"));
            Console.SetOut(s);
            try { vm.ProcessCustomerInteraction(); } catch (InvalidOperationException) { }
            Console.SetIn(prevIn);
            Console.SetOut(prevOut);

            // Assert
            Assert.Contains("Нужно ещё:", s.ToString());
        }

        // 5) Refund - Deposited
        [Fact]
        public void Refund_AfterDeposit_ReturnAndAsksForMoney()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;
            Console.SetIn(new StringReader("50\n"));
            Console.SetOut(new StringWriter());
            vm.AddMoney();

            // Act - Refund
            var sRefund = new StringWriter();
            Console.SetOut(sRefund);
            try { vm.Refund(); } catch (InvalidOperationException) { } 
            Console.SetOut(prevOut);

            // Assert 1 - there is a refund notice
            Assert.Contains("Возвращаем: 50 руб.", sRefund.ToString());

            // Act 2 - no refill (0)
            var sFlow = new StringWriter();
            Console.SetIn(new StringReader("0\n"));
            Console.SetOut(sFlow);
            try { vm.ProcessCustomerInteraction(); } catch (InvalidOperationException) { }
            Console.SetIn(prevIn);
            Console.SetOut(prevOut);

            // Assert 2
            Assert.Contains("ПОЖАЛУЙСТА, ВНЕСИТЕ ДЕНЬГИ", sFlow.ToString());
        }

        // 6) Refund - no add 
        [Fact]
        public void Refund_NoMoney_ResultNoMoneyMessage()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevOut = Console.Out;
            var s = new StringWriter();
            Console.SetOut(s);

            // Act
            try { vm.Refund(); } catch (InvalidOperationException) { } 
            Console.SetOut(prevOut);

            // Assert
            Assert.Contains("Нет денег для возврата", s.ToString());
        }

        // 7) Admin - password is wrong 
        [Fact]
        public void AdminMode_WrongPassword_ShowsError()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;
            var s = new StringWriter();
            Console.SetIn(new StringReader("wrong\n")); 
            Console.SetOut(s);

            // Act
            vm.AdminMode();

            // Assert
            Console.SetIn(prevIn);
            Console.SetOut(prevOut);
            Assert.Contains("Неверный пароль", s.ToString());
        }

        // 8) AdminMode restock - increases quantity 
        [Fact]
        public void AdminMode_Restock_PrintsAddQuantity()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;
            var s = new StringWriter();
            var input = new StringReader("admin123\n1\n1\n5\n0\n");

            // Act
            try
            {
                Console.SetIn(input);
                Console.SetOut(s);
                vm.AdminMode();
            }
            catch (InvalidOperationException) { }
            finally
            {
                Console.SetIn(prevIn);
                Console.SetOut(prevOut);
            }

            // Assert
            var output = s.ToString();
            Assert.Contains("Новый остаток: 15 шт.", output);
        }

        // 9) AdminMode collect - after a purchase, admin collects money 
        [Fact]
        public void AdminMode_Collect_PrintsCollectedAmount()
        {
            // Arrange
            var vm = new VendingMachine();
            var prevIn = Console.In;
            var prevOut = Console.Out;

            try
            {
                Console.SetIn(new StringReader("50\n"));
                Console.SetOut(new StringWriter());
                vm.AddMoney();

                Console.SetIn(new StringReader("8\n"));
                var sBuy = new StringWriter();
                Console.SetOut(sBuy);
                vm.ProcessCustomerInteraction();
            }
            catch (InvalidOperationException) { }
            finally
            {
                Console.SetIn(prevIn);
                Console.SetOut(prevOut);
            }

            var s = new StringWriter();
            var adminInput = new StringReader("admin123\n2\n0\n");

            // Act
            try
            {
                Console.SetIn(adminInput);
                Console.SetOut(s);
                vm.AdminMode();
            }
            catch (InvalidOperationException) { }
            finally
            {
                Console.SetIn(prevIn);
                Console.SetOut(prevOut);
            }

            // Assert
            var output = s.ToString();
            Assert.Contains("Снято:", output);
        }
    }
}
