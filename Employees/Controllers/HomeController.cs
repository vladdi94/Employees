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
        /// <param name="companyModel"></param>
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

            DepartmentModel? company = await _context.Departments.FirstOrDefaultAsync(x=>x.Name == companyModel.CompanyName);
            if (company == null)
            {
                company = new DepartmentModel() { Name = companyModel.CompanyName };
                _context.Departments.Add(company);
            }
            
            company.Employees.Add(emp);


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }

}
