using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShiftManager.Data;
using ShiftManager.Data.ViewModels;
using ShiftManager.Models;
using ShiftManager.Services.Interfaces;
using ShiftManager.Utilities;

namespace ShiftManager.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly AppDbContext db;
        AgeCalculator ageCalculator;

        public EmployeesService(AppDbContext db)
        {
            this.db = db;
            ageCalculator = new AgeCalculator();
        }

        public async Task AddAsync(EmployeeVM employee)
        {
            int age = ageCalculator.CalculateEmployeeAge(employee.DOB);
            var newEmployee = new Employee()
            {
                Name = employee.Name,
                DOB = employee.DOB,
                Age = age,
            };
            await db.Employees.AddAsync(newEmployee);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var employee = await db.Employees.Where(e => e.Id == Id).FirstOrDefaultAsync();
            if (employee != null)
            {
                db.Employees.Remove(employee);
            }
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await db.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int? Id)
        {
            return await db.Employees.Where(e => e.Id == Id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(EmployeeVM updatedEmployee)
        {
            var employee = await db.Employees.Where(n => n.Id == updatedEmployee.Id).FirstOrDefaultAsync();

            if (employee != null)
            {
                int age = ageCalculator.CalculateEmployeeAge(updatedEmployee.DOB);
                employee.Name = updatedEmployee.Name;
                employee.DOB = updatedEmployee.DOB;
                employee.Age = age;
                await db.SaveChangesAsync();
            }
        }
    }
}
