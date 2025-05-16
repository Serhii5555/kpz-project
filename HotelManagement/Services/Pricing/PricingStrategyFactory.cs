using HotelManagement.Models.Enums;

namespace HotelManagement.Services.Pricing
{
    public class PricingStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PricingStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPricingStrategy GetStrategy(PricingStrategyType strategyType)
        {
            return strategyType switch
            {
                PricingStrategyType.VIP => _serviceProvider.GetRequiredService<VipPricingStrategy>(),
                PricingStrategyType.Holiday => _serviceProvider.GetRequiredService<HolidayPricingStrategy>(),
                _ => _serviceProvider.GetRequiredService<StandardPricingStrategy>(),
            };
        }
    }
}
