namespace Ctr.AhphOcelot.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AhphClients
    {
        [Key]
        public int Id { get; set; }

        public int AbsoluteRefreshTokenLifetime { get; set; }

        public int AccessTokenLifetime { get; set; }

        public int AccessTokenType { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        public bool AllowOfflineAccess { get; set; }

        public bool AllowPlainTextPkce { get; set; }

        public bool AllowRememberConsent { get; set; }

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool AlwaysSendClientClaims { get; set; }

        public int AuthorizationCodeLifetime { get; set; }

        public bool? BackChannelLogoutSessionRequired { get; set; }

        [StringLength(500)]
        public string BackChannelLogoutUri { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientClaimsPrefix { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string ClientName { get; set; }

        [StringLength(100)]
        public string ClientUri { get; set; }

        public int? ConsentLifetime { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool EnableLocalLogin { get; set; }

        public bool Enabled { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; }

        [StringLength(100)]
        public string FrontChannelLogoutUri { get; set; }

        public int IdentityTokenLifetime { get; set; }

        public bool IncludeJwtId { get; set; }

        [StringLength(150)]
        public string LogoUri { get; set; }

        [StringLength(200)]
        public string PairWiseSubjectSalt { get; set; }

        [StringLength(50)]
        public string ProtocolType { get; set; }

        public int RefreshTokenExpiration { get; set; }

        public int RefreshTokenUsage { get; set; }

        public bool RequireClientSecret { get; set; }

        public bool RequireConsent { get; set; }

        public bool RequirePkce { get; set; }

        public int SlidingRefreshTokenLifetime { get; set; }

        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
    }
}
