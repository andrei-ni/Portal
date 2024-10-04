using System.ComponentModel.DataAnnotations.Schema;

namespace PhonebookManager.Models
{
    [Table ("All_employees")]
    public class Employee
    {
        public string? EmployeeID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Department { get; set; }
        public string? Title { get; set; }
        public string? Activity { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Manager { get; set; }
    }
}
