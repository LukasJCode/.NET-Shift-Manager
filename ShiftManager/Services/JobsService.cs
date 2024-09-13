using Microsoft.EntityFrameworkCore;
using ShiftManager.Data;
using ShiftManager.Data.ViewModels;
using ShiftManager.Models;
using ShiftManager.Services.Interfaces;

namespace ShiftManager.Services
{
    public class JobsService : IJobsService
    {
        private readonly AppDbContext db;

        public JobsService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(JobVM job)
        {
            var newJob = new Job()
            {
                Name = job.Name,
                RequiredAge = job.RequiredAge,
            };
            await db.Jobs.AddAsync(newJob);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var jobToDelete = await db.Jobs.Where(e => e.Id == Id).FirstOrDefaultAsync();
            if (jobToDelete != null)
            {
                db.Jobs.Remove(jobToDelete);
            }
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await db.Jobs.ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(int? Id)
        {
            return await db.Jobs.Where(e => e.Id == Id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(JobVM updatedJob)
        {
            var job = await db.Jobs.Where(e => e.Id == updatedJob.Id).FirstOrDefaultAsync();
            if (job != null)
            {
                job.Name = updatedJob.Name;
                job.RequiredAge = updatedJob.RequiredAge;
                await db.SaveChangesAsync();
            }
        }
    }
}
