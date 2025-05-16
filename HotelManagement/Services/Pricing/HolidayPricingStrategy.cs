using HotelManagement.Services.Holiday;

namespace HotelManagement.Services.Pricing
{
    public class HolidayPricingStrategy : IPricingStrategy
    {
        private readonly List<DateTime> _holidays;

        public HolidayPricingStrategy(IHolidaysProvider provider)
        {
            _holidays = provider.GetHolidays();
        }

        public decimal CalculatePrice(DateTime checkIn, DateTime checkOut, decimal baseRate)
        {
            decimal total = 0;
            for (var date = checkIn; date < checkOut; date = date.AddDays(1))
            {
                total += _holidays.Contains(date.Date) ? baseRate * 1.2m : baseRate;
            }
            return total;
        }
    }
}
