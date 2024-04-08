using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    /// <summary>
    /// EmployeePhones model for the Employees view.
    /// </summary>
    public class EmployeePhones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhoneNumberId { get; set; }

        //TODO: Add PhoneType Enum
        [Required(ErrorMessage = "{0} must be supplied")]
        [Display(Name = "Type", Description = "The Type of Phone Number")]
        [DataType(DataType.Text)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} must be between 1 and 15 characters.")]
        public required string PhoneType { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 10 characters.")]
        [RegularExpression(@"^(?:\d{10}|\d{3}-\d{3}-\d{4})$", ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone", Description = "Nullable; {0}")]
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Foreign Key for the EmployeeId.
        /// </summary>
        public int EmployeeId { get; set; }

        // Navigation property
        [ForeignKey("EmployeeId")]
        public Employees? Employees { get; set; }
    }
}