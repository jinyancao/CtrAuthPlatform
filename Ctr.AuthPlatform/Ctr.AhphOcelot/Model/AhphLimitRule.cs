namespace Ctr.AhphOcelot.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class AhphLimitRule
    {
        [Key]
        public int RuleId { get; set; }

        [Required]
        [StringLength(200)]
        public string LimitName { get; set; }

        [Required]
        [StringLength(50)]
        public string LimitPeriod { get; set; }

        public int LimitNum { get; set; }

        public int InfoStatus { get; set; }
    }
}
