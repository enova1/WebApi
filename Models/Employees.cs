using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Employees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")] public required string FirstName { get; set; }
        [Required(ErrorMessage = "{0} must be supplied")] public required string LastName { get; set; }
        [Required] public required DateTime HireDate { get; set; }

        public List<EmployeePhones>? EmployeePhones { get; set; }
        public List<EmployeeAddresses>? EmployeeAddresses { get; set; }
    }
}
