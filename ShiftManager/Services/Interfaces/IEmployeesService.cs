using ShiftManager.Data.ViewModels;
using ShiftManager.Models;

namespace ShiftManager.Services.Interfaces
{
    public interface IEmployeesService
    {
        Task<Employee> GetEmployeeByIdAsync(int? Id);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task AddAsync(EmployeeVM employee);
        Task UpdateAsync(EmployeeVM updatedEmployee);
        Task DeleteAsync(int Id);
    }
}
