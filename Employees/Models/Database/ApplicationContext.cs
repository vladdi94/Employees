using Microsoft.EntityFrameworkCore;

namespace Employees.Models.Database
{
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Сущность модели отделов
        /// </summary>
        public DbSet<DepartmentModel> Departments { get; set; } = null!;

        /// <summary>
        /// Сущность модели сотрудников
        /// </summary>
        public DbSet<EmployeeModel> Employees { get; set; } = null!;
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            // Создание базы данных при первом обращении
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentModel>().
                Property(x => x.Name).IsRequired();

            // При удалении отдела у модели сотрудников поле отдела в null
            modelBuilder.Entity<EmployeeModel>().
                HasOne(x=>x.DepartmentModel).
                WithMany(x=>x.Employees).
                OnDelete(DeleteBehavior.SetNull);
        }

        protected ApplicationContext()
        {
        }
    }
}
