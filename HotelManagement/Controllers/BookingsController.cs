using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using HotelManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var model = new Booking
            {
                room_id = roomId,
                check_in_date = checkInDate,
                check_out_date = checkOutDate,
            };

            var (guests, rooms) = await _bookingService.GetGuestsAndRoomsSelectListsAsync();
            ViewBag.Guests = guests;
            ViewBag.Rooms = rooms;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                var (guests, rooms) = await _bookingService.GetGuestsAndRoomsSelectListsAsync();
                ViewBag.Guests = guests;
                ViewBag.Rooms = rooms;
                return View(booking);
            }

            await _bookingService.CreateBookingAsync(booking);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var (guests, rooms) = await _bookingService.GetGuestsAndRoomsSelectListsAsync();
            ViewBag.Guests = guests;
            ViewBag.Rooms = rooms;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                await _bookingService.UpdateBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }

            var (guests, rooms) = await _bookingService.GetGuestsAndRoomsSelectListsAsync();
            ViewBag.Guests = guests;
            ViewBag.Rooms = rooms;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            await _bookingService.DeleteBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetBookedDates(int roomId)
        {
            var bookedDates = await _bookingService.GetBookedDatesAsync(roomId);

            if (bookedDates != null && bookedDates.Any())
            {
                return Json(new { success = true, bookedDates });
            }
            return Json(new { success = false, message = "No booked dates found." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableRooms()
        {
            var roomTypes = await _bookingService.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = new SelectList(roomTypes, nameof(RoomType.name), nameof(RoomType.name));

            return View(new RoomAvailabilityViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableRooms(RoomAvailabilityViewModel model)
        {
            var roomTypes = await _bookingService.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = new SelectList(roomTypes, nameof(RoomType.name), nameof(RoomType.name));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.AvailableRooms = await _bookingService.FindAvailableRoomsAsync(
                model.RoomType,
                model.check_in_date,
                model.check_out_date,
                model.PeopleCount
            );

            return View(model);
        }
    }
}