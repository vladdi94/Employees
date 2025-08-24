using Employees.Models.Database;
using Employees.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Employees.Controllers
{
    public class DepartmentController(
        ApplicationContext context, ILogger<DepartmentController> logger) : Controller
    {
        private readonly ApplicationContext _context = context;
        private readonly ILogger _logger = logger;

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentModel model)
        {

            DepartmentModel? department = await _context.Departments.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (department == null)
            {
                department = new DepartmentModel() { Name = model.Name };
                _context.Departments.Add(department);
            }
            else return BadRequest("Название отдела должно быть уникальным");           

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            // Передача данных в представление
            ViewBag.Department = new SelectList(await _context.Departments.Select(x => x.Name).ToListAsync());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentModel model)
        {
            var dep = await _context.Departments.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (dep != null)
            {
                _context.Departments.Remove(dep);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> List() => View(await _context.Departments.ToListAsync());


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var dep = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
            return View(dep);
        }
     
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentModel model)
        {
            var dep = await _context.Departments.FirstOrDefaultAsync(x=>x.Name == model.Name);
            if (dep != null)
                return BadRequest("Название отдела должно быть уникальным");

            var udep = await _context.Departments.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (udep != null)
            {
                udep.Name = model.Name;
                _context.Departments.Update(udep);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }
    }
}
