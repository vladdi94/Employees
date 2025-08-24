using System.ComponentModel.DataAnnotations;

namespace Employees.Models.Database
{
    /// <summary>
    /// Модель - сотрудник
    /// </summary>
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        [MaxLength(30)]
        public string? MiddleName { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Фотография сотрудника
        /// </summary>
        public byte[]? Photo { get; set; }

        // Поле внешнего ключа 
        public int? DepartmentModelId { get; set; }

        /// <summary>
        /// Родительское навигационное свойство
        /// </summary>
        public DepartmentModel? DepartmentModel { get; set; }
    }
}
