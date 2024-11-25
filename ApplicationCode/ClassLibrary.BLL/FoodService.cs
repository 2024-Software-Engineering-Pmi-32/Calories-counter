using WpfAppCaloriesCounter.DAL; 

namespace WpfAppCaloriesCounter.BLL
{
    public class FoodService
    {
        private readonly ApplicationDbContext _context;

        public FoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FoodEntry> GetFoodEntries()
        {
            return _context.FoodEntries.ToList();
        }

        public void AddFoodEntry(FoodEntry entry)
        {
            _context.FoodEntries.Add(entry);
            _context.SaveChanges();
        }

        public void DeleteFoodEntry(int id)
        {
            var entry = _context.FoodEntries.Find(id);
            if (entry != null)
            {
                _context.FoodEntries.Remove(entry);
                _context.SaveChanges();
            }
        }
    }
}
