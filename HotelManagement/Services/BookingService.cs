using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using HotelManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IGuestRepository _guestRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IRoomTypeRepository _roomTypeRepo;

        public BookingService(IBookingRepository bookingRepo, IGuestRepository guestRepo, IRoomRepository roomRepo, IRoomTypeRepository roomTypeRepo)
        {
            _bookingRepo = bookingRepo;
            _guestRepo = guestRepo;
            _roomRepo = roomRepo;
            _roomTypeRepo = roomTypeRepo;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepo.GetAllBookingsAsync();
        }

        public async Task<(SelectList Guests, SelectList Rooms)> GetGuestsAndRoomsSelectListsAsync()
        {
            var guests = await _guestRepo.GetAllGuestsAsync();
            var rooms = await _roomRepo.GetAllRoomsAsync();
            return (
                new SelectList(guests, "guest_id", "guest_display"),
                new SelectList(rooms, "room_id", "room_number")
            );
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            await _bookingRepo.CreateBookingAsync(booking);
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _bookingRepo.GetBookingByIdAsync(id);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _bookingRepo.UpdateBookingAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepo.DeleteBookingAsync(id);
        }

        public async Task<IEnumerable<DateTime>> GetBookedDatesAsync(int roomId)
        {
            return await _bookingRepo.GetBookedDatesAsync(roomId);
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _roomTypeRepo.GetAllRoomTypesAsync();
        }

        public async Task<IEnumerable<Room>> FindAvailableRoomsAsync(string roomType, DateTime checkInDate, DateTime checkOutDate, int peopleCount)
        {
            return await _roomRepo.FindAvailableRoomsAsync(roomType, checkInDate, checkOutDate, peopleCount);
        }
    }
}
