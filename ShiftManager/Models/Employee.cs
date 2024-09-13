using System.ComponentModel.DataAnnotations;

namespace ShiftManager.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
    }
}
