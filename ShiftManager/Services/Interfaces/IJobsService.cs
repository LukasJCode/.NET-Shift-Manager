using ShiftManager.Data.ViewModels;
using ShiftManager.Models;

namespace ShiftManager.Services.Interfaces
{
    public interface IJobsService
    {
        Task<Job> GetJobByIdAsync(int? Id);
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task AddAsync(JobVM job);
        Task UpdateAsync(JobVM updatedJob);
        Task DeleteAsync(int Id);
    }
}
