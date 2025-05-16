namespace HotelManagement.Services.Pricing
{
    public class VipPricingStrategy : IPricingStrategy
    {
        public decimal CalculatePrice(DateTime checkIn, DateTime checkOut, decimal baseRate)
        {
            int nights = (checkOut - checkIn).Days;
            return nights * baseRate * 0.9m;
        }
    }
}
