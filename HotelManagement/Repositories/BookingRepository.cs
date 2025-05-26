using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace HotelManagement.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseContext _database;

        private const string BookingSelectQuery = @"
            SELECT 
                b.booking_id, 
                b.guest_id, 
                b.room_id, 
                b.check_in_date, 
                b.check_out_date, 
                b.status, 
                b.total_price, 
                b.payment_status, 
                r.room_number AS display_room_name, 
                CONCAT(g.first_name, ' ', g.last_name) AS display_guest_name
            FROM 
                Bookings b
            LEFT JOIN Rooms r ON b.room_id = r.room_id
            LEFT JOIN Guests g ON b.guest_id = g.guest_id";

        public BookingRepository(DatabaseContext database)
        {
            _database = database;
        }

        private async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object? parameters = null)
        {
            using var connection = _database.CreateConnection();
            return await connection.QueryAsync<T>(query, parameters);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await ExecuteQueryAsync<Booking>(BookingSelectQuery);
        }

        public async Task<Booking?> GetBookingByIdAsync(int booking_id)
        {
            var query = BookingSelectQuery + " WHERE b.booking_id = @booking_id";
            var result = await ExecuteQueryAsync<Booking>(query, new { booking_id });
            return result.FirstOrDefault();
        }

        public async Task<int> CreateBookingAsync(Booking booking)
        {
            var query = @"INSERT INTO Bookings 
                          (guest_id, room_id, check_in_date, check_out_date, status, total_price, payment_status)
                          VALUES 
                          (@guest_id, @room_id, @check_in_date, @check_out_date, @status, @total_price, @payment_status); 
                          SELECT CAST(SCOPE_IDENTITY() AS INT)";
            using var connection = _database.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, booking);
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            var query = @"UPDATE Bookings 
                          SET guest_id = @guest_id, room_id = @room_id, 
                              check_in_date = @check_in_date, check_out_date = @check_out_date, 
                              status = @status, total_price = @total_price, 
                              payment_status = @payment_status 
                          WHERE booking_id = @booking_id";
            using var connection = _database.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, booking);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBookingAsync(int booking_id)
        {
            var query = "DELETE FROM Bookings WHERE booking_id = @booking_id";
            using var connection = _database.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { booking_id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPaymentStatusAsync(string paymentStatus)
        {
            var query = BookingSelectQuery + " WHERE b.payment_status = @paymentStatus";
            return await ExecuteQueryAsync<Booking>(query, new { paymentStatus });
        }

        private IEnumerable<string> ExpandDates(DateTime start, DateTime end)
        {
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                yield return date.ToString("yyyy-MM-dd");
            }
        }

        public async Task<IEnumerable<string>> GetBookedDatesAsync(int roomId)
        {
            var query = @"
                SELECT check_in_date, check_out_date 
                FROM Bookings 
                WHERE room_id = @roomId AND status IN ('Booked', 'Pending')";
            var bookings = await ExecuteQueryAsync<dynamic>(query, new { roomId });

            var bookedDates = new List<string>();
            foreach (var booking in bookings)
                bookedDates.AddRange(ExpandDates(booking.check_in_date, booking.check_out_date));

            return bookedDates;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
        {
            var query = BookingSelectQuery + " WHERE b.status = @status";
            return await ExecuteQueryAsync<Booking>(query, new { status });
        }
    }
}
