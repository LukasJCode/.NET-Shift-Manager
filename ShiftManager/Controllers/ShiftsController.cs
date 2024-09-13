using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShiftManager.Data.ViewModels;
using ShiftManager.Models;
using ShiftManager.Services.Interfaces;
using ShiftManager.Utilities;

namespace ShiftManager.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly IShiftsService _service;
        private readonly IEmployeesService _employeeService;
        private readonly IJobsService _jobsService;
        private AgeCalculator ageCalculator;

        public ShiftsController(IShiftsService service, IEmployeesService employeeService, IJobsService jobsService)
        {
            _service = service;
            _employeeService = employeeService;
            _jobsService = jobsService;
            ageCalculator = new AgeCalculator();
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllShiftsAsync();
            return View(data);
        }

        //GET
        public async Task<IActionResult> Create()
        {
            var shiftDropdowns = await _service.GetShiftDropdowns();

            ViewBag.Employees = new SelectList(shiftDropdowns.Employees, "Id", "Name");
            ViewBag.Jobs = new SelectList(shiftDropdowns.Jobs, "Id", "Name");

            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(ShiftVM shift)
        {
            List<string> JobsEmployeeIsTooYoungFor = new List<string>();
            Employee emp = await _employeeService.GetEmployeeByIdAsync(shift.EmployeeId);

            //Checking if employee fits the age requirement for all the jobs
            foreach (var taskId in shift.JobIds)
            {
                Job job = await _jobsService.GetJobByIdAsync(taskId);
                if (ageCalculator.CalculateEmployeeAge(emp.DOB) < job.RequiredAge)
                {
                    JobsEmployeeIsTooYoungFor.Add(job.Name);
                }
            }

            //Displaying jobs employee is too young for if there are any
            if (JobsEmployeeIsTooYoungFor.Count() > 0)
            {
                string message = "";
                foreach (var job in JobsEmployeeIsTooYoungFor)
                {
                    message += job + ", ";
                }

                ModelState.AddModelError("Too Young", emp.Name + " is too young for task(s): " + message);
            }


            //Making sure shift doesnt start after it has ended
            if (shift.ShiftStart > shift.ShiftEnd)
            {
                ModelState.AddModelError("Shift creation error", "Shift cannot start after shift has ended");
            }

            if (!ModelState.IsValid)
            {
                var shiftDropdowns = await _service.GetShiftDropdowns();

                ViewBag.Employees = new SelectList(shiftDropdowns.Employees, "Id", "Name");
                ViewBag.Jobs = new SelectList(shiftDropdowns.Jobs, "Id", "Name");

                return View(shift);
            }

            await _service.AddAsync(shift);
            return RedirectToAction("Index");
        }

        //GET
        public async Task<IActionResult> Edit(int id)
        {
            var shiftDetails = await _service.GetShiftByIdAsync(id);
            if (shiftDetails == null)
            {
                return View("NotFound");
            }

            var response = new ShiftVM()
            {
                ShiftStart = shiftDetails.ShiftStart,
                ShiftEnd = shiftDetails.ShiftEnd,
                EmployeeId = shiftDetails.EmployeeId,
                JobIds = shiftDetails.Jobs_Shifts.Select(x => x.JobId).ToList()
            };

            var shiftDropdowns = await _service.GetShiftDropdowns();

            ViewBag.Employees = new SelectList(shiftDropdowns.Employees, "Id", "Name");
            ViewBag.Jobs = new SelectList(shiftDropdowns.Jobs, "Id", "Name");

            return View(response);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ShiftVM shift)
        {
            if (id != shift.Id)
            {
                return View("NotFound");
            }

            List<string> JobsEmployeeIsTooYoungFor = new List<string>();
            Employee emp = await _employeeService.GetEmployeeByIdAsync(shift.EmployeeId);

            //Checking if employee fits the age requirement for all the jobs
            foreach (var taskId in shift.JobIds)
            {
                Job job = await _jobsService.GetJobByIdAsync(taskId);
                if (ageCalculator.CalculateEmployeeAge(emp.DOB) < job.RequiredAge)
                {
                    JobsEmployeeIsTooYoungFor.Add(job.Name);
                }
            }

            //Displaying jobs employee is too young for if there are any
            if (JobsEmployeeIsTooYoungFor.Count() > 0)
            {
                string message = "";
                foreach (var job in JobsEmployeeIsTooYoungFor)
                {
                    message += job + ", ";
                }

                ModelState.AddModelError("Too Young", emp.Name + " is too young for task(s): " + message);
            }

            //Making sure shift doesnt start after it has ended
            if (shift.ShiftStart > shift.ShiftEnd)
            {
                ModelState.AddModelError("Shift creation error", "Shift cannot start after shift has ended");
            }

            if (!ModelState.IsValid)
            {
                var shiftDropdowns = await _service.GetShiftDropdowns();

                ViewBag.Employees = new SelectList(shiftDropdowns.Employees, "Id", "Name");
                ViewBag.Jobs = new SelectList(shiftDropdowns.Jobs, "Id", "Name");

                return View(shift);
            }

            await _service.UpdateAsync(shift);
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
