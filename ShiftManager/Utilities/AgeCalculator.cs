namespace ShiftManager.Utilities
{
    public class AgeCalculator
    {
        public int CalculateEmployeeAge(DateTime DOB)
        {
            var birthday = DOB.Date;
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;

            if (birthday.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
