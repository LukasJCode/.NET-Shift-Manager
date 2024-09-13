using Microsoft.AspNetCore.Mvc;
using ShiftManager.Data.ViewModels;
using ShiftManager.Services.Interfaces;

namespace ShiftManager.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobsService _service;

        public JobsController(IJobsService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllJobsAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allJobs = await _service.GetAllJobsAsync();

            if (!string.IsNullOrEmpty(searchString))
            {

                var filteredResult = allJobs.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allJobs);
        }


        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(JobVM job)
        {
            if (!ModelState.IsValid)
            {
                return View(job);
            }
            await _service.AddAsync(job);
            return RedirectToAction("Index");
        }

        //GET
        public async Task<IActionResult> Edit(int id)
        {
            var jobDetails = await _service.GetJobByIdAsync(id);
            if (jobDetails == null)
            {
                return View("NotFound");
            }

            var response = new JobVM()
            {
                Name = jobDetails.Name,
                RequiredAge = jobDetails.RequiredAge
            };

            return View(response);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Edit(JobVM job)
        {
            if (!ModelState.IsValid)
            {
                return View(job);
            }
            await _service.UpdateAsync(job);
            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
