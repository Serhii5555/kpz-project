namespace HotelManagement.Services.Pricing
{
    public class BookingPriceCalculator
    {
        private readonly IPricingStrategy _strategy;

        public BookingPriceCalculator(IPricingStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CalculatePrice(DateTime checkIn, DateTime checkOut, decimal baseRate)
        {
            return _strategy.CalculatePrice(checkIn, checkOut, baseRate);
        }
    }
}
