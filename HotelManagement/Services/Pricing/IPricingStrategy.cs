namespace HotelManagement.Services.Pricing
{
    public interface IPricingStrategy
    {
        decimal CalculatePrice(DateTime checkIn, DateTime checkOut, decimal baseRate);
    }
}
