using System.ComponentModel.DataAnnotations;

namespace Employees.Models.DTO
{
    public class CompanyModel
    {
        private const int maxLength = 30;
        
        [Display(Name ="Название отдела")]               
        public string? CompanyName { get; set; }
       

        [Display(Name = "Имя")]        
        [StringLength(maxLength, ErrorMessage = $"Превышено допустимое число символов в строке")]
        [Required(ErrorMessage="Не указано имя")]
        public string FirstName { get; set; }


        [Display(Name ="Фамилия")]
        [StringLength(maxLength, ErrorMessage = $"Превышено допустимое число символов в строке")]
        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }


        [Display(Name ="Отчество")]
        [StringLength(maxLength, ErrorMessage = $"Превышено допустимое число символов в строке")]
        public string? MiddleName { get; set; }


        [Display(Name ="Номер телефона")]
        [RegularExpression(@"^\+\d+$", ErrorMessage ="Номер телефона должен начинаться с + и состоять только из цифр")]
        [StringLength(20, MinimumLength =10, ErrorMessage = "Неверная длина строки")]
        [Required(ErrorMessage = "Не указан номер телефона")]
        public string PhoneNumber { get; set; }


        [Display(Name ="Фотография сотрудника")]       
        public byte[]? Photo { get; set; }
        
        public int? EditId { get; set; }
    }
}
