using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class EmployeePhones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhoneNumberId { get; set; }

        //TODO: Add PhoneType Enum
        [Required(ErrorMessage = "{0} must be supplied")] public required string PhoneType { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")] public required string PhoneNumber { get; set; }

        // Foreign Key
        public int EmployeeId { get; set; }
    }
}
