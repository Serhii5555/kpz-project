using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HotelManagement.Models.ValueObjects
{
    public class EmailAddress
    {
        private readonly string _value;
        
        private EmailAddress(string value)
        {
            _value = value;
        }
        
        public static EmailAddress Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;
                
            email = email.Trim().ToLowerInvariant();
            
            if (email.Length > 100)
                throw new ArgumentException("Email cannot exceed 100 characters.");
                
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email address format.");
                
            return new EmailAddress(email);
        }
        
        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
        
        public string Value => _value;
        
        public override string ToString() => _value;
        public override bool Equals(object obj) => obj is EmailAddress other && _value == other._value;
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        
        public static implicit operator string(EmailAddress email) => email?._value;
    }
    
    public class PhoneNumber
    {
        private readonly string _value;
        
        private PhoneNumber(string value)
        {
            _value = value;
        }
        
        public static PhoneNumber Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.");
                
            phoneNumber = phoneNumber.Trim();
            
            if (phoneNumber.Length > 20)
                throw new ArgumentException("Phone number cannot exceed 20 characters.");
                
            if (!IsValidPhoneNumber(phoneNumber))
                throw new ArgumentException("Invalid phone number format.");
                
            return new PhoneNumber(phoneNumber);
        }
        
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^\+?[0-9\s-]+$");
            return phoneRegex.IsMatch(phoneNumber);
        }
        
        public string Value => _value;
        public string Normalized => Regex.Replace(_value, @"[\s-]", "");
        
        public override string ToString() => _value;
        public override bool Equals(object obj) => obj is PhoneNumber other && _value == other._value;
        public override int GetHashCode() => _value.GetHashCode();
        
        public static implicit operator string(PhoneNumber phone) => phone._value;
    }
    
    public class PassportCode
    {
        private readonly string _value;
        
        private PassportCode(string value)
        {
            _value = value;
        }
        
        public static PassportCode Create(string passportCode)
        {
            if (string.IsNullOrWhiteSpace(passportCode))
                throw new ArgumentException("Passport code is required.");
                
            passportCode = passportCode.Trim().ToUpperInvariant();
            
            if (passportCode.Length > 20)
                throw new ArgumentException("Passport code cannot exceed 20 characters.");
                
            if (!IsValidPassportCode(passportCode))
                throw new ArgumentException("Passport code can only contain letters and numbers.");
                
            return new PassportCode(passportCode);
        }
        
        private static bool IsValidPassportCode(string passportCode)
        {
            var passportRegex = new Regex(@"^[A-Za-z0-9]+$");
            return passportRegex.IsMatch(passportCode);
        }
        
        public string Value => _value;
        
        public override string ToString() => _value;
        public override bool Equals(object obj) => obj is PassportCode other && _value == other._value;
        public override int GetHashCode() => _value.GetHashCode();
        
        public static implicit operator string(PassportCode passport) => passport._value;
    }
    
    public class DateOfBirth
    {
        private readonly DateTime _value;
        
        private DateOfBirth(DateTime value)
        {
            _value = value;
        }
        
        public static DateOfBirth Create(DateTime dateOfBirth)
        {
            if (dateOfBirth == default(DateTime))
                throw new ArgumentException("Date of birth is required.");
                
            if (dateOfBirth > DateTime.Today)
                throw new ArgumentException("Date of birth cannot be in the future.");
                
            var age = CalculateAge(dateOfBirth);
            if (age < 0 || age > 150)
                throw new ArgumentException("Invalid date of birth - age must be between 0 and 150 years.");
                
            return new DateOfBirth(dateOfBirth);
        }
        
        public static DateOfBirth Create(string dateString)
        {
            if (!DateTime.TryParse(dateString, out DateTime date))
                throw new ArgumentException("Please enter a valid date.");
                
            return Create(date);
        }
        
        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
        
        public DateTime Value => _value;
        public int Age => CalculateAge(_value);
        public bool IsMinor => Age < 18;
        
        public override string ToString() => _value.ToString("yyyy-MM-dd");
        public override bool Equals(object obj) => obj is DateOfBirth other && _value == other._value;
        public override int GetHashCode() => _value.GetHashCode();
        
        public static implicit operator DateTime(DateOfBirth dateOfBirth) => dateOfBirth._value;
    }
}


using HotelManagement.Models.ValueObjects;
using HotelManagement.Validations;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Guest
    {
        [Key] 
        public int guest_id { get; set; }
        
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string first_name { get; set; }
        
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string last_name { get; set; }
        
        private EmailAddress _email;
        public EmailAddress Email
        {
            get => _email;
            set => _email = value;
        }
        
 
        public string email
        {
            get => _email?.Value;
            set => _email = string.IsNullOrWhiteSpace(value) ? null : EmailAddress.Create(value);
        }
        
        private PhoneNumber _phoneNumber;
        [UniquePhoneNumber(ErrorMessage = "This phone number is already in use.")]
        public PhoneNumber PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value;
        }
        
  
        public string phone_number
        {
            get => _phoneNumber?.Value;
            set => _phoneNumber = string.IsNullOrWhiteSpace(value) ? null : PhoneNumber.Create(value);
        }
        
        private PassportCode _passportCode;
        public PassportCode PassportCode
        {
            get => _passportCode;
            set => _passportCode = value;
        }
        
 
        public string passport_code
        {
            get => _passportCode?.Value;
            set => _passportCode = string.IsNullOrWhiteSpace(value) ? null : PassportCode.Create(value);
        }
        
        private DateOfBirth _dateOfBirth;
        public DateOfBirth DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value;
        }
        

        public DateTime date_of_birth
        {
            get => _dateOfBirth?.Value ?? default(DateTime);
            set => _dateOfBirth = value == default(DateTime) ? null : DateOfBirth.Create(value);
        }
        
        [Required(ErrorMessage = "Nationality is required.")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters.")]
        public string nationality { get; set; }
        
        public string guest_display => $"{first_name} {last_name} | {guest_id}";
        
        public bool IsMinor => _dateOfBirth?.IsMinor ?? false;
        public int? Age => _dateOfBirth?.Age;
        

        public static Guest CreateGuest(string firstName, string lastName, string email, 
            string phoneNumber, string passportCode, DateTime dateOfBirth, string nationality)
        {
            var guest = new Guest
            {
                first_name = firstName?.Trim(),
                last_name = lastName?.Trim(),
                nationality = nationality?.Trim()
            };
            
   
            guest.Email = string.IsNullOrWhiteSpace(email) ? null : EmailAddress.Create(email);
            guest.PhoneNumber = PhoneNumber.Create(phoneNumber);
            guest.PassportCode = PassportCode.Create(passportCode);
            guest.DateOfBirth = DateOfBirth.Create(dateOfBirth);
            
            return guest;
        }
    }
}