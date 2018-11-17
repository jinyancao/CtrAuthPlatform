namespace Ctr.AhphOcelot.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AhphAuthGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(500)]
        public string GroupDetail { get; set; }

        public int InfoStatus { get; set; }
    }
}
