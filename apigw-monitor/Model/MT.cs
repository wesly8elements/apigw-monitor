using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace apigw_monitor.Model
{
    [Table("MT", Schema = "dbo")]
    public class MT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int PortalId { get; set; }

        [Required]
        public int CarrierId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Shortcode { get; set; }

        [Required]
        [MaxLength(35)]
        public string Msisdn { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        [MaxLength(100)]
        public string? SessionId { get; set; }

        [Required]
        public int RequestStatus { get; set; }

        public int? DeliveryStatus { get; set; }

        [MaxLength(1000)]
        public string? Remarks { get; set; }
    }
}
