using Microsoft.AspNetCore.Mvc;
using ShiftManager.Data.ViewModels;
using ShiftManager.Services.Interfaces;

namespace ShiftManager.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _service;

        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllEmployeesAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allEmployees = await _service.GetAllEmployeesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {

                var filteredResult = allEmployees.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allEmployees);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeVM employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            await _service.AddAsync(employee);
            return RedirectToAction("Index");
        }

        //GET
        public async Task<IActionResult> Edit(int id)
        {
            var employeeDetails = await _service.GetEmployeeByIdAsync(id);
            if (employeeDetails == null)
            {
                return View("NotFound");
            }

            var response = new EmployeeVM()
            {
                Name = employeeDetails.Name,
                DOB = employeeDetails.DOB,
            };

            return View(response);
        }

        //Post
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeVM employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            await _service.UpdateAsync(employee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
