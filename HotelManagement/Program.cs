using HotelManagement.Models;
using HotelManagement.Repositories;
using HotelManagement.Repositories.Interfaces;
using HotelManagement.Services.Holiday;
using HotelManagement.Services.Pricing;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton(new DatabaseContext(connectionString));
builder.Services.AddSingleton<IGuestRepository, GuestRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
builder.Services.AddSingleton<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddSingleton<IServiceRepository, ServiceRepository>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPaymentServiceRepository, PaymentServiceRepository>();
builder.Services.AddSingleton<IStatisticsRepository, StatisticsRepository>();

// Booking strategy implementation
builder.Services.AddSingleton<IHolidaysProvider, DefaultHolidaysProvider>();

builder.Services.AddScoped<StandardPricingStrategy>();
builder.Services.AddScoped<VipPricingStrategy>();
builder.Services.AddScoped<HolidayPricingStrategy>();

builder.Services.AddScoped<PricingStrategyFactory>();

builder.Services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        options.MaxModelValidationErrors = 50;
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => "The field is required.");
    });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
