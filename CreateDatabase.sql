CREATE DATABASE HotelManagement;
GO

USE HotelManagement;
GO

CREATE TABLE Room_Types (
    room_type_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(20) NOT NULL,
    description NVARCHAR(MAX),
    base_price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Guests (
    guest_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    email NVARCHAR(100),  
    phone_number NVARCHAR(20) UNIQUE NOT NULL, 
    passport_code NVARCHAR(20) NOT NULL,
    date_of_birth DATE NOT NULL,
    nationality NVARCHAR(50) NOT NULL
);

CREATE TABLE Rooms (
    room_id INT IDENTITY(1,1) PRIMARY KEY,
    room_number NVARCHAR(10) UNIQUE NOT NULL,
    room_type_id INT NOT NULL,
    availability BIT NOT NULL,
    bed_count INT,
    features NVARCHAR(MAX),
    FOREIGN KEY (room_type_id) REFERENCES Room_Types(room_type_id) ON DELETE CASCADE
);

CREATE TABLE Bookings (
    booking_id INT IDENTITY(1,1) PRIMARY KEY,
    guest_id INT NOT NULL,
    room_id INT NOT NULL,
    check_in_date DATE NOT NULL,
    check_out_date DATE NOT NULL,
    status NVARCHAR(20) NOT NULL CHECK (status IN ('Booked', 'Pending', 'Completed')),  
    total_price DECIMAL(10, 2),
    payment_status NVARCHAR(20) NOT NULL CHECK (payment_status IN ('Pending', 'Completed')), 
    FOREIGN KEY (guest_id) REFERENCES Guests(guest_id),
    FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
);

CREATE TABLE Services (
    service_id INT IDENTITY(1,1) PRIMARY KEY,
    service_name NVARCHAR(50) NOT NULL,
    description NVARCHAR(MAX),
    price DECIMAL(10, 2) NOT NULL 
);

CREATE TABLE Payments (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT NOT NULL,
    payment_amount DECIMAL(10, 2) NOT NULL,
    payment_date DATETIME2(7) NOT NULL,
    payment_type NVARCHAR(20) NOT NULL CHECK (payment_type IN ('Hotel', 'Service')), 
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id) ON DELETE CASCADE
);

CREATE TABLE Payment_Services (
    payment_service_id INT IDENTITY(1,1) PRIMARY KEY,
    payment_id INT NOT NULL,
    service_id INT NOT NULL,
    service_amount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (payment_id) REFERENCES Payments(payment_id) ON DELETE CASCADE,
    FOREIGN KEY (service_id) REFERENCES Services(service_id) ON DELETE CASCADE
);
GO

CREATE PROCEDURE CalculateBookingCost
    @BookingId INT
AS
BEGIN
    SELECT 
        b.booking_id,
        DATEDIFF(DAY, b.check_in_date, b.check_out_date) + 1 AS stay_days,  
        rt.base_price,
        (DATEDIFF(DAY, b.check_in_date, b.check_out_date) + 1) * rt.base_price AS total_cost  
    FROM 
        Bookings b
    INNER JOIN 
        Rooms r ON b.room_id = r.room_id
    INNER JOIN 
        Room_Types rt ON r.room_type_id = rt.room_type_id
    WHERE 
        b.booking_id = @BookingId;
END;
GO

CREATE PROCEDURE FindAvailableRooms
    @RoomType NVARCHAR(20),
    @CheckInDate DATE,
    @CheckOutDate DATE,
    @PeopleCount INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.room_id, 
        r.room_number, 
        rt.name AS room_type, 
        r.bed_count, 
        r.features
    FROM Rooms r
    JOIN Room_Types rt ON r.room_type_id = rt.room_type_id
    WHERE rt.name = @RoomType 
      AND r.bed_count >= @PeopleCount
      AND r.room_id NOT IN (
            SELECT b.room_id
            FROM Bookings b
            WHERE b.status IN ('Booked', 'Pending') 
              AND NOT (
                  @CheckOutDate < b.check_in_date OR
                  @CheckInDate > b.check_out_date 
              )
      )
    ORDER BY r.room_number; 
END;
GO

CREATE PROCEDURE GetRevenueForPeriod
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        SUM(B.total_price) AS TotalRevenue
    FROM 
        Bookings B
    WHERE 
        B.check_in_date >= @StartDate
        AND B.check_out_date <= @EndDate;
END;
GO

CREATE TRIGGER UpdateBookingPaymentStatus
ON Payments
AFTER INSERT
AS
BEGIN
    UPDATE Bookings
    SET payment_status = 'Completed'
    FROM Bookings b
    INNER JOIN inserted i ON b.booking_id = i.booking_id
    WHERE i.payment_type = 'Hotel' AND b.payment_status = 'Pending';
END;
GO

CREATE TRIGGER UpdateBookingStatusOnCheckin
ON Bookings
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE Bookings
    SET status = 'Pending'
    WHERE check_in_date = CAST(GETDATE() AS DATE) 
      AND status = 'Booked';
END;
GO

CREATE TRIGGER UpdateBookingStatusOnCheckout
ON Bookings
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE Bookings
    SET status = 'Completed'
    WHERE check_out_date <= CAST(GETDATE() AS DATE)
      AND status IN ('Booked', 'Pending');
END;
GO

CREATE TRIGGER UpdateRoomAvailabilityOnBookingDeletion
ON Bookings
AFTER DELETE
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM Bookings
        WHERE room_id IN (SELECT room_id FROM deleted)
        AND status = 'Pending'  
    )
    BEGIN
        UPDATE Rooms
        SET availability = 1
        WHERE room_id IN (SELECT room_id FROM deleted);
    END
END;
GO

CREATE TRIGGER SetRoomAvailabilityOnCheckIn
ON Bookings
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @room_id INT;
    DECLARE @check_in_date DATE;
    
    SELECT @room_id = room_id, @check_in_date = check_in_date
    FROM inserted;

    IF @check_in_date <= CAST(GETDATE() AS DATE)
    BEGIN
        UPDATE Rooms
        SET availability = 0
        WHERE room_id = @room_id;
    END
END
GO

CREATE TRIGGER SetRoomAvailabilityOnCheckOut
ON Bookings
AFTER UPDATE
AS
BEGIN
    DECLARE @room_id INT;
    DECLARE @check_out_date DATE;
    DECLARE @status NVARCHAR(20);
    
    SELECT @room_id = room_id, @check_out_date = check_out_date, @status = status
    FROM inserted;

    IF @check_out_date <= CAST(GETDATE() AS DATE) AND @status = 'Completed'
    BEGIN
        UPDATE Rooms
        SET availability = 1
        WHERE room_id = @room_id;
    END
END;
GO

CREATE TRIGGER UpdateTotalPriceAfterPayment
ON Payments
AFTER INSERT
AS
BEGIN
    DECLARE @payment_id INT;
    DECLARE @payment_amount DECIMAL(10, 2);
    DECLARE @booking_id INT;
    DECLARE @total_price DECIMAL(10, 2);

    SELECT @payment_id = payment_id, @payment_amount = payment_amount, @booking_id = booking_id
    FROM inserted;

    SELECT @total_price = total_price
    FROM Bookings
    WHERE booking_id = @booking_id;

    UPDATE Bookings
    SET total_price = @total_price + @payment_amount
    WHERE booking_id = @booking_id;
END;
GO