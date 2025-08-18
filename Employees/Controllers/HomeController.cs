using Employees.Models.Database;
using Employees.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        [HttpGet]
        public async Task<IActionResult> Index() => View(await _context.Employees.Include(x=>x.DepartmentModel).ToListAsync());

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Передача данных в представление
            ViewBag.Department = new SelectList(await _context.Departments.Select(x => x.Name).ToListAsync());

            return View();
        }

        [HttpGet]
        public IActionResult CreateDepartment() => View();

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

            // Добавление сотрудника в отдел
            DepartmentModel? department = await _context.Departments.FirstOrDefaultAsync(x => x.Name == companyModel.CompanyName);
            department?.Employees.Add(emp);

            /*DepartmentModel? department = await _context.Departments.FirstOrDefaultAsync(x=>x.Name == companyModel.CompanyName);
            if (department == null)
            {
                department = new DepartmentModel() { Name = companyModel.CompanyName };
                _context.Departments.Add(department);
            }
            
            department.Employees.Add(emp);*/




            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создание отдела
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CompanyModel companyModel)
        {

            DepartmentModel? department = await _context.Departments.FirstOrDefaultAsync(x => x.Name == companyModel.CompanyName);
            if (department == null)
            {
                department = new DepartmentModel() { Name = companyModel.CompanyName };
                _context.Departments.Add(department);
            }

            //department.Employees.Add(emp);

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

        [HttpGet]
        public async Task<IActionResult> DeleteDepartment()
        {
            // Передача данных в представление
            ViewBag.Department = new SelectList(await _context.Departments.Select(x => x.Name).ToListAsync());

            return View();
        }

        /// <summary>
        /// Удалить отдел
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(CompanyModel companyModel)
        {
            var dep = await _context.Departments.FirstOrDefaultAsync(x=>x.Name==companyModel.CompanyName);
            if (dep != null)
            {
                _context.Departments.Remove(dep);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ListDepartment()
        {          
            return View(await _context.Departments.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(int ?id)
        {
            var dep = await _context.Departments.FirstOrDefaultAsync(x=>x.Id==id);
            return View(dep);
        }

        /// <summary>
        /// Редактирование отдела
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]       
        public async Task<IActionResult> EditDepartment(DepartmentModel model)
        {
            var dep = await _context.Departments.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (dep != null)
            {
                dep.Name = model.Name;
                _context.Departments.Update(dep);

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
            // Передача данных в представление
            ViewBag.Department = new SelectList(await _context.Departments.Select(x => x.Name).ToListAsync());

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
                /*
                if (department == null)
                {
                    department = new DepartmentModel() { Name = company.CompanyName };
                    _context.Departments.Add(department);
                }

                department.Employees.Add(emp);*/

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
