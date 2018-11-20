namespace Ctr.AhphOcelot.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class AhphClientReRouteWhiteList
    {
        [Key]
        public int WhiteListId { get; set; }

        public int? ReRouteId { get; set; }

        public int? Id { get; set; }
    }
}
