namespace Ctr.AhphOcelot.Model
{
    using System.ComponentModel.DataAnnotations;
    public partial class AhphReRoutesItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; }

        [StringLength(500)]
        public string ItemDetail { get; set; }

        public int? ItemParentId { get; set; }

        public int InfoStatus { get; set; }
    }
}
