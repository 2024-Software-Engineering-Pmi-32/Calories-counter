using CaloriesCounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double CurrentWeight { get; set; } // Поточна вага (в кг)
        public double Height { get; set; } // Зріст (в см)
        public int Age { get; set; } // Вік
        public string Gender { get; set; } // Стать ("Чоловік", "Жінка")
        public string ActivityLevel { get; set; } // Рівень активності ("Низький", "Середній", "Високий")
        public List<Product> AddedProducts { get; set; } = new List<Product>();
        public List<string> Goals { get; set; } = new List<string>();
    }
}