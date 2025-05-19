using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<(SelectList Guests, SelectList Rooms)> GetGuestsAndRoomsSelectListsAsync();
        Task CreateBookingAsync(Booking booking);

        Task<Booking> GetBookingByIdAsync(int id);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);

        Task<IEnumerable<DateTime>> GetBookedDatesAsync(int roomId);

        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();

        Task<IEnumerable<Room>> FindAvailableRoomsAsync(string roomType, DateTime checkInDate, DateTime checkOutDate, int peopleCount);
    }
}
