namespace HotelManagement.Services.Holiday
{
    public class DefaultHolidaysProvider : IHolidaysProvider
    {
        public List<DateTime> GetHolidays()
        {
            int currentYear = DateTime.Today.Year;

            return new List<DateTime>
            {
                new DateTime(currentYear, 12, 31), // Переддень Нового Року
                new DateTime(currentYear, 1, 1),   // Новий Рік
                new DateTime(currentYear, 3, 8),   // Міжнародний жіночий день
                new DateTime(currentYear, 8, 24),  // День Незалежності України
            };
        }
    }
}
