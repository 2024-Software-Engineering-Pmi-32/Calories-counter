using System;
using System.Linq;
using System.Collections.Generic;
using CaloriesCounter.BLL;
using Microsoft.EntityFrameworkCore;

namespace CaloriesCounter
{
    public class CaloriesService
    {
        private readonly CaloriesCounterContext _context;

        public CaloriesService(CaloriesCounterContext context)
        {
            _context = context;
        }

        public User Register(string name, string email, string password)
        {
            var user = new User { Name = name, Email = email, Password = password };
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine("Реєстрація успішна.");
            return user;
        }

        public User Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                Console.WriteLine($"Ласкаво просимо, {user.Name}!");
                return user;
            }
            Console.WriteLine("Неправильний логін або пароль.");
            return null;
        }

        //public void AddProductToUser(int userId, string productName, int calories)
        //{
        //    var user = _context.Users.Find(userId);
        //    if (user != null)
        //    {
        //        var product = new Product { Name = productName, Calories = calories };
        //        user.AddedProducts.Add(product);
        //        _context.Products.Add(product);
        //        _context.SaveChanges();
        //        Console.WriteLine("Продукт успішно додано.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Користувача не знайдено.");
        //    }
        //}

        public void AddProductToUser(
        int userId,
        string productName,
        int calories,
        double? proteins = null,
        double? fats = null,
        double? carbohydrates = null,
        bool isDrink = false,
        bool isDish = false,
        List<(string IngredientName, int IngredientCalories, double IngredientProteins, double IngredientFats, double IngredientCarbohydrates)> ingredients = null)
        {
            var user = _context.Users.Include(u => u.AddedProducts).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Product product;

                if (isDish && ingredients != null && ingredients.Any())
                {
                    // Обчислення калорійності і макроелементів для страви
                    int totalCalories = ingredients.Sum(i => i.IngredientCalories);
                    double totalProteins = ingredients.Sum(i => i.IngredientProteins);
                    double totalFats = ingredients.Sum(i => i.IngredientFats);
                    double totalCarbohydrates = ingredients.Sum(i => i.IngredientCarbohydrates);

                    product = new Product
                    {
                        Name = productName,
                        Calories = totalCalories,
                        Proteins = totalProteins,
                        Fats = totalFats,
                        Carbohydrates = totalCarbohydrates,
                        IsDish = true,
                        IsDrink = false,
                        Ingredients = ingredients.Select(i => new Product
                        {
                            Name = i.IngredientName,
                            Calories = i.IngredientCalories,
                            Proteins = i.IngredientProteins,
                            Fats = i.IngredientFats,
                            Carbohydrates = i.IngredientCarbohydrates
                        }).ToList()
                    };
                }
                else
                {
                    // Для простого продукту або напою, використовуємо значення за замовчуванням для макроелементів, якщо вони не були вказані
                    product = new Product
                    {
                        Name = productName,
                        Calories = calories,
                        Proteins = proteins ?? 0, // Якщо білки не вказані, встановлюємо значення 0
                        Fats = fats ?? 0,         // жири так само
                        Carbohydrates = carbohydrates ?? 0, // вуглеводи так само
                        IsDish = false,
                        IsDrink = isDrink
                    };
                }

                user.AddedProducts.Add(product);
                _context.Products.Add(product);
                _context.SaveChanges();

                Console.WriteLine($"{(isDish ? "Страву" : isDrink ? "Напій" : "Продукт")} успішно додано.");
            }
            else
            {
                Console.WriteLine("Користувача не знайдено.");
            }
        }

        public void CalculateTotalCalories(int userId)
        {
            var user = _context.Users.Include(u => u.AddedProducts).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                int totalCalories = user.AddedProducts.Sum(p => p.Calories);
                Console.WriteLine($"Загальна кількість калорій для користувача {user.Name}: {totalCalories}");
            }
            else
            {
                Console.WriteLine("Користувача не знайдено.");
            }
        }

        public void SetGoal(int userId, string goal, double targetWeight)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                //BMI
                double heightInMeters = user.Height / 100.0;
                double minHealthyWeight = 18.5 * heightInMeters * heightInMeters;
                double maxHealthyWeight = 24.9 * heightInMeters * heightInMeters;


                if (targetWeight < minHealthyWeight || targetWeight > maxHealthyWeight)
                {
                    Console.WriteLine($"Цільова вага повинна бути у здорових межах ({minHealthyWeight:F1}-{maxHealthyWeight:F1} кг).");
                    return;
                }

                // Оновлення цілі користувача
                user.Goals.Clear();
                string goalMessage = "";
                if (goal == "Схуднення")
                {
                    if (user.CurrentWeight > targetWeight)
                    {
                        goalMessage = $"Схуднення до {targetWeight} кг.";
                    }
                    else
                    {
                        Console.WriteLine("Ваша поточна вага вже менша або дорівнює цільовій.");
                        return;
                    }
                }
                else if (goal == "Набір ваги")
                {
                    if (user.CurrentWeight < targetWeight)
                    {
                        goalMessage = $"Набір ваги до {targetWeight} кг.";
                    }
                    else
                    {
                        Console.WriteLine("Ваша поточна вага вже більша або дорівнює цільовій.");
                        return;
                    }
                }
                else if (goal == "Підтримка форми")
                {
                    if (user.CurrentWeight >= minHealthyWeight && user.CurrentWeight <= maxHealthyWeight)
                    {
                        goalMessage = $"Підтримка форми у межах {minHealthyWeight:F1}-{maxHealthyWeight:F1} кг.";
                    }
                    else
                    {
                        Console.WriteLine("Ваша поточна вага знаходиться поза межами здорової ваги.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Невірна ціль.");
                    return;
                }

                user.Goals.Add(goalMessage);
                _context.SaveChanges();
                Console.WriteLine($"Ціль '{goalMessage}' успішно встановлено.");
            }
            else
            {
                Console.WriteLine("Користувача не знайдено.");
            }
        }

        public void ViewCalorieHistory(int userId)
        {
            var user = _context.Users.Include(u => u.AddedProducts).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Console.WriteLine($"Історія калорій для {user.Name}:");
                foreach (var product in user.AddedProducts)
                {
                    Console.WriteLine($"{product.Name} - {product.Calories} калорій");
                }
            }
            else
            {
                Console.WriteLine("Користувача не знайдено.");
            }
        }

        public void UpdateUserInfo(int userId, string newName, string newEmail)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.Name = newName;
                user.Email = newEmail;
                _context.SaveChanges();
                Console.WriteLine("Інформацію про користувача успішно оновлено.");
            }
            else
            {
                Console.WriteLine("Користувача не знайдено.");
            }
        }

        public void GetRecommendations(int userId)
        {
            var user = _context.Users.Include(u => u.AddedProducts).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine("Користувача не знайдено.");
                return;
            }

            //BMR
            double BMR = user.Gender == "Жінка"
                ? 447.6 + (9.2 * user.CurrentWeight) + (3.1 * user.Height) - (4.3 * user.Age)
                : 88.36 + (13.4 * user.CurrentWeight) + (4.8 * user.Height) - (5.7 * user.Age);


            //потреба в калоріях
            double totalCalorieNeed;
            if (user.ActivityLevel == "Низький")
            {
                totalCalorieNeed = BMR * 1.2;
            }
            else if (user.ActivityLevel == "Середній")
            {
                totalCalorieNeed = BMR * 1.55;
            }
            else if (user.ActivityLevel == "Високий")
            {
                totalCalorieNeed = BMR * 1.9;
            }
            else
            {
                totalCalorieNeed = BMR * 1.2;
            }



            int totalCalories = user.AddedProducts.Sum(p => p.Calories);


            Console.WriteLine($"Рекомендації для {user.Name}:");
            Console.WriteLine($"Поточна вага: {user.CurrentWeight} кг. Рекомендована калорійність: {totalCalorieNeed:F0} ккал.");

            if (user.Goals.Any(g => g.Contains("Схуднення")) && user.CurrentWeight > totalCalorieNeed - 500)
            {
                Console.WriteLine("- Для схуднення рекомендуємо обмежити калорії на 500-700 ккал від добової потреби.");
            }
            else if (user.Goals.Any(g => g.Contains("Набір ваги")) && user.CurrentWeight < totalCalorieNeed + 500)
            {
                Console.WriteLine("- Для набору ваги рекомендуємо збільшити калорії на 300-500 ккал від добової потреби.");
            }
            else if (user.Goals.Any(g => g.Contains("Підтримка форми")))
            {
                Console.WriteLine("- Для підтримки форми тримайте калорії в межах добових потреб.");
            }


            Console.WriteLine("- Пийте більше води.");
            Console.WriteLine("- Регулярно займайтеся спортом.");
        }

        public void CommunicateWithAdmin(int userId, string message)
        {

            Console.WriteLine($"Повідомлення від користувача {userId} до адміністратора: {message}");
        }

        public void CommunicateWithClient(int adminId, int userId, string message)
        {

            Console.WriteLine($"Повідомлення від адміністратора {adminId} до користувача {userId}: {message}");
        }

        public void UpdateProductDatabase(int productId, string newName, int newCalories)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.Name = newName;
                product.Calories = newCalories;
                _context.SaveChanges();
                Console.WriteLine("Інформацію про продукт успішно оновлено.");
            }
            else
            {
                Console.WriteLine("Продукт не знайдено.");
            }
        }

        public void SearchProducts(string searchTerm)
        {
            var results = _context.Products.Where(p => p.Name.Contains(searchTerm)).ToList();
            if (results.Any())
            {
                Console.WriteLine("Знайдені продукти:");
                foreach (var product in results)
                {
                    Console.WriteLine($"{product.Name} - {product.Calories} калорій");
                }
            }
            else
            {
                Console.WriteLine("Продукти не знайдені.");
            }
        }
    }

}


