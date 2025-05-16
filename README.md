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

### 📌 Programming Principles

This project follows several key programming principles to ensure clean, maintainable, and scalable code:

1. **Single Responsibility Principle (SRP)** – Each class and method has one clearly defined responsibility.
2. **Separation of Concerns (SoC)** – Business logic, data access, and UI layers are decoupled.
3. **Don’t Repeat Yourself (DRY)** – Reusable methods and centralized logic reduce duplication.
4. **Open/Closed Principle (OCP)** – Code is open for extension but closed for modification.
5. **Dependency Inversion Principle (DIP)** – High-level modules are not dependent on low-level modules; both depend on abstractions.
6. **KISS (Keep It Simple, Stupid)** – Solutions are implemented in the simplest way possible without unnecessary complexity.

---

### 🧠 Design Patterns

The following design patterns have been applied in the project:

1. **Repository Pattern**  
   Used to abstract and encapsulate data access logic.  
   _Location:_ [`HotelManagement/Repositories/BookingRepository.cs`](./HotelManagement/Repositories/BookingRepository.cs)

2. **Dependency Injection**  
   Injected services and repositories via constructor injection for better testability and flexibility.  
   _Configured in:_ [`HotelManagement/Program.cs`](./HotelManagement/Program.cs)

3. **Model-View-Controller (MVC)**  
   Standard ASP.NET MVC architecture separates data (Models), UI (Views), and logic (Controllers).  
   _Example:_ [`HotelManagement/Controllers/BookingController.cs`](./HotelManagement/Controllers/BookingController.cs)

4. **Strategy Pattern**  
   Used for dynamic room pricing based on booking type (Standard, VIP, Holiday).  
   _Location:_ [`HotelManagement/Services/Pricing/`](./HotelManagement/Services/Pricing/)

---

### 🔄 Refactoring Techniques

The following refactoring techniques were applied to improve code quality:

1. **Introduce Interface** – Common operations abstracted into interfaces for flexibility.
2. **Replace Magic Strings/Numbers with Constants** – Defined enums/constants for validation patterns.
3. **Rename for Clarity** – Renamed variables and methods for better readability.
4. **Extract Method** – Long methods broken into smaller, reusable methods.
5. **Encapsulate Field** – Replaced direct field access with properties and validation logic.

---

### 🧱 Project Structure

The solution follows a layered architecture:

```

HotelManagement/
│
├── Controllers/          # MVC Controllers (UI logic)
├── Models/               # Data models and DTOs
├── Views/                # Razor Views (UI templates)
├── Data/                 # Database communication (Dapper)
├── Repositories/         # Dapper data access logic
├── Services/             # Services that support functionality
├── Validations/          # Custom validation attributes for data annotation
├── wwwroot/              # Static files (CSS, JS)
├── tailwind.config.js    # Tailwind CSS configuration
├── appsettings.json      # Application config (DB connection, etc.)
└── Program.cs            # App entry point and service configuration

```

---

### 💻 Technologies Used

- **ASP.NET MVC (.NET 8)** – Web framework
- **Dapper** – Lightweight ORM for SQL Server
- **SQL Server** – Relational database
- **Tailwind CSS** – Utility-first CSS framework
- **npm** – For managing frontend packages
- **Visual Studio** – Development environment

---

