using System.Globalization;

namespace Falcon.Libraries.Common.Object
{
    public class ConfigObject
    {
        public JwtObject? JWT { get; set; }
        public ConfigUrl? Url { get; set; }
        public CultureInfo? CultureInfo { get; set; }
        public AuthenticationSettings AuthenticationSettings { get; set; } = new AuthenticationSettings();
        public string? ApplicationCode { get; set; }
    }

    public class ConfigUrl
    {
        public string? Api { get; set; }
        public string? ApiGeneral { get; set; }
    }
    public class ParentConfigObject
    {
        public ConfigObject? Config { get; set; }
    }
    public class AuthenticationSettings
    {
        public string? LoginPath { get; set; }
        public string? LogoutPath { get; set; }
        public string? ReturnUrlParameter { get; set; }
        public int ExpireTimeSpan { get; set; }
        public bool HttpOnly { get; set; }
        public bool SlidingExpiration { get; set; }
    }
    public class JwtObject
    {
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
        public string? Secret { get; set; }
        public int ExpireHour { get; set; }
    }
}
