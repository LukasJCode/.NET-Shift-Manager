using ShiftManager.Data.ViewModels;
using ShiftManager.Models;

namespace ShiftManager.Services.Interfaces
{
    public interface IShiftsService
    {
        Task<Shift> GetShiftByIdAsync(int? Id);
        Task<IEnumerable<Shift>> GetAllShiftsAsync();
        Task AddAsync(ShiftVM shift);
        Task UpdateAsync(ShiftVM updatedShift);
        Task DeleteAsync(int Id);
        Task<ShiftDropDownsVM> GetShiftDropdowns();
    }
}
