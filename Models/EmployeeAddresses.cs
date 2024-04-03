using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class EmployeeAddresses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")] public required string Address1{ get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")] public required string City { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")] public required string State { get; set; }

        [Required(ErrorMessage = "{0} must be supplied")] public required string ZipCode { get; set; }

        // Foreign Key
        public int EmployeeId { get; set; }

    }
}
