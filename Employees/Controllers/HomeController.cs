using Employees.Models;
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

        /// <summary>
        /// Пагинация
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page = 1)
        {
                       
            int pageSize = 50;

            IQueryable<EmployeeModel> employees = _context.Employees.Include(x => x.DepartmentModel);
            var count = await employees.CountAsync();
            var items = await employees.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new (count, page, pageSize);
            IndexViewModel viewModel = new (items, pageViewModel);
            return View(viewModel);
        }

        /// <summary>
        /// Фильтрация
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EmployeesList(string searchTerm)
        {
            var company = await _context.Employees.Include(x=>x.DepartmentModel)
                .Where(p => string.IsNullOrEmpty(searchTerm) ||
                    p.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                    p.LastName.ToLower().Contains(searchTerm.ToLower()) ||
                    (p.MiddleName != null && p.MiddleName.ToLower().Contains(searchTerm.ToLower())) ||
                    (p.PhoneNumber != null && p.PhoneNumber.ToLower().Contains(searchTerm.ToLower())))
                .ToListAsync();

            return PartialView("_EmployeeList", company);
        }      
    }

}
