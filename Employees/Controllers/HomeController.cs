using Employees.Models.Database;
using Employees.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employees.Controllers
{
    /// <summary>
    /// Конструктор контроллера для подключения контекста данных БД
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public class HomeController(ApplicationContext context, ILogger<HomeController> logger) : Controller
    {
        private readonly ApplicationContext _context = context;
        private readonly ILogger _logger = logger;
        
        // применение энергичной загрузки навигационных свойств
        public async Task<IActionResult> Index() => View(await _context.Employees.Include(x=>x.DepartmentModel).ToListAsync());

        public IActionResult Create() => View();

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="companyModel">Промежуточная модель для передачи данных</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CompanyModel companyModel)
        {
            var emp = new EmployeeModel()
            {
                FullName = companyModel.FullName,
                PhoneNuber = companyModel.PhoneNuber,
                Photo = companyModel.Photo
            };
            
            _context.Employees.Add(emp);

            DepartmentModel? department = await _context.Departments.FirstOrDefaultAsync(x=>x.Name == companyModel.CompanyName);
            if (department == null)
            {
                department = new DepartmentModel() { Name = companyModel.CompanyName };
                _context.Departments.Add(department);
            }
            
            department.Employees.Add(emp);


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x=>x.Id == id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        /// <summary>
        /// Метод возвращает данные для редактирования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int ?id)
        {
            if (id != null)
            {
                var emp = await _context.Employees.Include(x => x.DepartmentModel).FirstOrDefaultAsync(x=>x.Id==id);
                if (emp != null)
                    return View(new CompanyModel()
                    {
                        Photo = emp.Photo,
                        CompanyName = emp.DepartmentModel?.Name,
                        EditId = emp.Id,
                        FullName = emp.FullName,
                        PhoneNuber = emp.PhoneNuber
                    });
            }
            return NotFound();
        }

        /// <summary>
        /// Метод получает уже отредактированные данные
        /// </summary>
        /// <param name="company">Модель</param>
        /// <param name="id">ID редактированной модели</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(CompanyModel company, int? id)
        {
            try
            {
                // Поиск модели
                var emp = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

                var department = await _context.Departments.FirstOrDefaultAsync(x => x.Name == company.CompanyName);

                if (emp?.DepartmentModelId != department?.Id)
                {
                    emp.DepartmentModelId = department?.Id;
                    
                }

                _context.Employees.Update(emp);

                if (department == null)
                {
                    department = new DepartmentModel() { Name = company.CompanyName };
                    _context.Departments.Add(department);
                }

                department.Employees.Add(emp);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return await Task.FromResult(BadRequest());
            }
        }
    }

}
