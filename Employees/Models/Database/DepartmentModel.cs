using System.ComponentModel.DataAnnotations;

namespace Employees.Models.Database
{
    /// <summary>
    /// Модель - отдел
    /// </summary>
    public class DepartmentModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название отдела
        /// </summary>
        public string? Name { get; set; }

        // Дочернее навигационное свойство с типом ICollection
        /// <summary>
        /// Список сотрудников в отделе
        /// </summary>
        public List<EmployeeModel> Employees { get; set; } = [];
    }
}
