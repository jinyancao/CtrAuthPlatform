namespace Ctr.AhphOcelot.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class AhphConfigReRoutes
    {
        [Key]
        public int CtgRouteId { get; set; }

        public int? AhphId { get; set; }

        public int? ReRouteId { get; set; }
    }
}
