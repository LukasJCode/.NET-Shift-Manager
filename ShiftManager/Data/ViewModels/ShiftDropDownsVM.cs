using ShiftManager.Models;

namespace ShiftManager.Data.ViewModels
{
    public class ShiftDropDownsVM
    {
        public List<Employee> Employees { get; set; }
        public List<Job> Jobs { get; set; }
        public ShiftDropDownsVM()
        {
            Employees = new List<Employee>();
            Jobs = new List<Job>();
        }
    }
}
