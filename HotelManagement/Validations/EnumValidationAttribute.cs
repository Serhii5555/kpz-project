using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Validations
{
    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidationAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return Enum.IsDefined(_enumType, value.ToString());
        }
    }

}
