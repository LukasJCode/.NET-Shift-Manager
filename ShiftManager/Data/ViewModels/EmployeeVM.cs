using System.ComponentModel.DataAnnotations;

namespace ShiftManager.Data.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Required]
        public DateTime DOB { get; set; }
    }
}
