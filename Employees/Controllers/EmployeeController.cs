using Employees.Models.Database;
using Employees.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Employees.Controllers
{
    public class EmployeeController(ApplicationContext context, ILogger<HomeController> logger) : Controller
    {
        private readonly ApplicationContext _context = context;
        private readonly ILogger _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Передача данных в представление
            ViewBag.Department = new SelectList(await _context.Departments.Select(x => x.Name).ToListAsync());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyModel companyModel, IFormFile file)
        {
            try
            {
                var emps = await _context.Employees.FirstOrDefaultAsync(x => x.PhoneNumber == companyModel.PhoneNumber);
                if (emps != null)
                    return BadRequest("Номер телефона должен быть уникальным");

                var emp = new EmployeeModel()
                {
                    FirstName = companyModel.FirstName,
                    LastName = companyModel.LastName,
                    MiddleName = companyModel.MiddleName,
                    PhoneNumber = companyModel.PhoneNumber,
                    //Photo = companyModel.Photo
                };

                // Обработка фото
                if (file != null)
                {
                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    emp.Photo = stream.ToArray();
                }

                _context.Employees.Add(emp);

                // Добавление сотрудника в отдел
                DepartmentModel? department = await _context.Departments.FirstOrDefaultAsync(x => x.Name == companyModel.CompanyName);
                department?.Employees.Add(emp);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Ошибка при создании модели");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {                      
            // Передача данных в представление
            ViewBag.Department = new SelectList(await _context.Departments.Select(x => x.Name).ToListAsync());

            if (id != null)
            {
                var emp = await _context.Employees.Include(x => x.DepartmentModel).FirstOrDefaultAsync(x => x.Id == id);
                if (emp != null)
                {                    
                    return View(new CompanyModel()
                    {
                        Photo = emp.Photo,
                        CompanyName = emp.DepartmentModel?.Name,
                        EditId = emp.Id,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        MiddleName = emp.MiddleName,
                        PhoneNumber = emp.PhoneNumber
                    });

                }    
                    
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyModel company, int? id, IFormFile file)
        {
            try
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(x => x.PhoneNumber == company.PhoneNumber && x.Id != id);
                if (emp != null)
                    return BadRequest("Номер телефона должен быть уникальным");

                // Поиск модели
                emp = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

                var department = await _context.Departments.FirstOrDefaultAsync(x => x.Name == company.CompanyName);

                if (emp?.DepartmentModelId != department?.Id)
                    emp.DepartmentModelId = department?.Id;

                emp.PhoneNumber = company.PhoneNumber;
                emp.LastName = company.LastName;

                // Обработка фото
                if (file != null)
                {
                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    emp.Photo = stream.ToArray();
                }

                _context.Employees.Update(emp);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Ошибка при редактировании модели");
            }
        }

      
    }

}
