namespace Ctr.AhphOcelot.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class AhphReRoute
    {
        [Key]
        public int ReRouteId { get; set; }

        public int? ItemId { get; set; }

        [Required]
        [StringLength(150)]
        public string UpstreamPathTemplate { get; set; }

        [Required]
        [StringLength(50)]
        public string UpstreamHttpMethod { get; set; }

        [StringLength(100)]
        public string UpstreamHost { get; set; }

        [Required]
        [StringLength(50)]
        public string DownstreamScheme { get; set; }

        [Required]
        [StringLength(200)]
        public string DownstreamPathTemplate { get; set; }

        [StringLength(500)]
        public string DownstreamHostAndPorts { get; set; }

        [StringLength(300)]
        public string AuthenticationOptions { get; set; }

        [StringLength(100)]
        public string RequestIdKey { get; set; }

        [StringLength(200)]
        public string CacheOptions { get; set; }

        [StringLength(100)]
        public string ServiceName { get; set; }

        [StringLength(500)]
        public string LoadBalancerOptions { get; set; }

        [StringLength(200)]
        public string QoSOptions { get; set; }

        [StringLength(200)]
        public string DelegatingHandlers { get; set; }

        public int? Priority { get; set; }

        public int InfoStatus { get; set; }
    }
}
