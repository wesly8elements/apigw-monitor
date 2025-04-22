using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace apigw_monitor.Model
{
    [Table("SubscriberTransaction2", Schema = "dbo")]
    public class SubscriberTransaction2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public int? SubscriptionID { get; set; }

        [Required]
        public int PortalID { get; set; }

        public byte? SubscriptionTypeID { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Msisdn { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(5)]
        public string Currency { get; set; }

        [Required]
        public byte IsPaid { get; set; }

        [Required]
        public int Type { get; set; }

        [MaxLength(150)]
        public string? Remarks { get; set; }

        [Required]
        public long TransactionID { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal? NetAmount { get; set; }
    }

}
