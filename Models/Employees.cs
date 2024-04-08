using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    /// <summary>
    /// Employees model for the Employees view. 
    /// </summary>
    public class Employees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")]
        [Display(Name = "First Name", Description = "Employee First Name")]
        [DataType(DataType.Text)]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} must be between 1 and 15 characters.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")]
        [Display(Name = "Last Name", Description = "Employee Last Name")]
        [DataType(DataType.Text)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} must be between 1 and 15 characters.")]
        public required string LastName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Hire Date", Description = "Date of the Employee was hired on.")]
        [Required] public required DateTime HireDate { get; set; }

        // Navigation property
        public List<EmployeePhones>? EmployeePhones { get; set; }
        public List<EmployeeAddresses>? EmployeeAddresses { get; set; }
    }
}