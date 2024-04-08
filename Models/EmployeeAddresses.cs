using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    /// <summary>
    /// EmployeeAddresses model for the Employees view.
    /// </summary>
    public class EmployeeAddresses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")]
        [Display(Name = "Address 1", Description = "Street Address 1", GroupName = "Address")]
        [DataType(DataType.Text)]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 35 characters.")]
        public string? Address1 { get; set; }


        [Display(Name = "Address 1", Description = "Street Address 2", GroupName = "Address")]
        [DataType(DataType.Text)]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 35 characters.")]
        public string? Address2 { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        [DataType(DataType.Text)]
        [Display(Name = "City", Description = "Employee City", GroupName = "Address")]
        public string? City { get; set; }

        [StringLength(2, MinimumLength = 1, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        [DataType(DataType.Text)]
        [Display(Name = "State", Description = "Employee State", GroupName = "Address")]
        public string? State { get; set; }

        [StringLength(2, MinimumLength = 1, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        [DataType(DataType.Text)]
        [Display(Name = "Zip Code", Description = "Employee Zip", GroupName = "Address")]
        [Required] public required string ZipCode { get; set; }

        /// <summary>
        /// Foreign Key for the EmployeeId.
        /// </summary>
        public int EmployeeId { get; set; }

        // Navigation property
        [ForeignKey("EmployeeId")]
        public Employees? Employees { get; set; }
    }
}