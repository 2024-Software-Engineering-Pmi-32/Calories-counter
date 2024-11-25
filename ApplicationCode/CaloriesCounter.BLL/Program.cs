using CaloriesCounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new CaloriesCounterContext())
        {
            var service = new CaloriesService(context);

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\nДоступні дії:");
                Console.WriteLine("1. Додати продукт");
                Console.WriteLine("2. Підрахувати загальну кількість калорій");
                Console.WriteLine("3. Пошук продукту");
                Console.WriteLine("4. Вийти");
                Console.Write("Оберіть дію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть ID користувача: ");
                        int userId = int.Parse(Console.ReadLine());
                        Console.Write("Введіть назву продукту: ");
                        string productName = Console.ReadLine();
                        Console.Write("Введіть кількість калорій: ");
                        int calories = int.Parse(Console.ReadLine());

                        service.AddProductToUser(userId, productName, calories);
                        break;

                    case "2":
                        Console.Write("Введіть ID користувача: ");
                        userId = int.Parse(Console.ReadLine());
                        service.CalculateTotalCalories(userId);
                        break;

                    case "3":
                        Console.Write("Введіть назву для пошуку: ");
                        string searchTerm = Console.ReadLine();
                        service.SearchProducts(searchTerm);
                        break;

                    case "4":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Невірна команда.");
                        break;
                }
            }
        }
    }
}