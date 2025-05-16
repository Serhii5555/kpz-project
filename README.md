# 🏨 Hotel Booking Management System

## 🎯 Functionality Overview

### 1. User Interface (UI)

#### Home Page:

* Hotel statistics dashboard
* Sidebar navigation element that links to operations for bookings, guests, rooms, and more.

#### Booking Management Page (example for other entity pages, which are similar):

* Table displaying all bookings

  * Columns: Guest, Room, Dates, Status, Payment
* Actions:

  * Create (including a quick option with available room search)
  * Search
  * Edit
  * Delete

#### Booking Creation Form (example for other forms, which are similar):

* Form to create a booking

  * Select guest
  * Select room
  * Set check-in and check-out dates
* Input validation
* "Create Booking" button

---

### 2. Business Logic

#### Booking:

* Availability check for selected room and dates
* Automatic update of booking status (`Pending`, `Booked`, `Completed`)
* Total price calculation
* Hotel-wide statistics aggregation

#### Payment:

* Payment type selection: `Hotel` or `Service`
* Validation of payment status: `Pending`, `Completed`

#### Data Validation:

* Server-side validation of required fields
* Duplicate entry protection
* Error handling for invalid input

---

### 3. Database Operations

* Dapper is used for SQL Server database access
* SQL queries optimized for CRUD operations
* All queries encapsulated via the `Repository` layer
* Connection string managed via `appsettings.json`

---

### 4. Administrative Features

* Full CRUD control over all entities (rooms, bookings, guests)

---

## 🚀 How to Run the Project Locally

### 🧾 1. Clone the Repository

```bash
git clone https://github.com/Serhii5555/kpz-project
cd kpz-project
````

---

### 🗄️ 2. Set Up the Database

#### 📄 Run SQL Script

Open `CreateDatabase.sql` in **SQL Server Management Studio** and execute it to create the necessary database schema.

> Make sure to adjust the SQL Server instance name if necessary.

---

### ⚙️ 3. Configure the Connection String

Open `appsettings.json` and make sure your connection string points to the correct SQL Server instance and database:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=HotelManagement;Trusted_Connection=True;"
}
```

---

### 🧑‍💻 4. Install Frontend Dependencies

TailwindCSS and other assets are managed with npm.

```bash
cd HotelManagement
npm install
```

---

### ▶️ 5. Run the Application

#### Visual Studio

1. Open the `.sln` file in Visual Studio.
2. Set `HotelManagement` as the startup project (if not already).
3. Press `F5` to build and run.

The app will be available at:

```
https://localhost:7150
```

---

## 🛠️ Technical Overview

Coming soon...