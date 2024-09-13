using Microsoft.EntityFrameworkCore;
using ShiftManager.Data;
using ShiftManager.Data.ViewModels;
using ShiftManager.Models;
using ShiftManager.Services.Interfaces;

namespace ShiftManager.Services
{
    public class ShiftsService : IShiftsService
    {
        private readonly AppDbContext db;

        public ShiftsService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(ShiftVM shift)
        {
            var newShift = new Shift()
            {
                ShiftStart = shift.ShiftStart,
                ShiftEnd = shift.ShiftEnd,
                EmployeeId = shift.EmployeeId,
            };
            await db.Shifts.AddAsync(newShift);
            await db.SaveChangesAsync();

            //Add Jobs to Shift
            foreach (var jobId in shift.JobIds)
            {
                var newJobShift = new Job_Shift()
                {
                    JobId = jobId,
                    ShiftId = newShift.Id
                };
                await db.Shifts_Jobs.AddAsync(newJobShift);
            }
            await db.SaveChangesAsync();

        }

        public async Task DeleteAsync(int Id)
        {
            var shiftToDelete = await db.Shifts.Where(s => s.Id == Id).FirstOrDefaultAsync();
            if (shiftToDelete != null)
            {
                db.Shifts.Remove(shiftToDelete);
            }
            await db.SaveChangesAsync();

            //Remove existing Shifts
            var existingShifts = db.Shifts_Jobs.Where(n => n.ShiftId == Id).ToList();
            db.Shifts_Jobs.RemoveRange(existingShifts);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
        {
            var shifts = await db.Shifts
                .Include(e => e.Employee)
                .Include(js => js.Jobs_Shifts).ThenInclude(j => j.Job)
                .ToListAsync();

            return shifts;
        }

        public async Task<Shift> GetShiftByIdAsync(int? Id)
        {
            var shiftDetails = await db.Shifts
                .Include(e => e.Employee)
                .Include(js => js.Jobs_Shifts).ThenInclude(j => j.Job)
                .FirstOrDefaultAsync(n => n.Id == Id);

            return shiftDetails;
        }

        public async Task UpdateAsync(ShiftVM updatedShift)
        {
            var shift = await db.Shifts.Where(e => e.Id == updatedShift.Id).FirstOrDefaultAsync();
            if (shift != null)
            {
                shift.ShiftStart = updatedShift.ShiftStart;
                shift.ShiftEnd = updatedShift.ShiftEnd;
                shift.EmployeeId = updatedShift.EmployeeId;
                await db.SaveChangesAsync();
            }

            //Remove existing Shifts
            var existingShifts = db.Shifts_Jobs.Where(n => n.ShiftId == updatedShift.Id).ToList();
            db.Shifts_Jobs.RemoveRange(existingShifts);
            await db.SaveChangesAsync();

            //Add Jobs to Shift
            foreach (var jobId in updatedShift.JobIds)
            {
                var newJobShift = new Job_Shift()
                {
                    JobId = jobId,
                    ShiftId = shift.Id
                };
                await db.Shifts_Jobs.AddAsync(newJobShift);
            }
            await db.SaveChangesAsync();
        }

        public async Task<ShiftDropDownsVM> GetShiftDropdowns()
        {
            var response = new ShiftDropDownsVM()
            {
                Employees = await db.Employees.ToListAsync(),
                Jobs = await db.Jobs.ToListAsync(),
            };

            return response;
        }
    }
}
