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

        /// <summary>
        /// ФИО
        /// </summary>        
        public string? FullName { get; set; }

        /// <summary>
        /// Номер телефона сотрудника
        /// </summary>
        public string? PhoneNuber { get; set; }

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
