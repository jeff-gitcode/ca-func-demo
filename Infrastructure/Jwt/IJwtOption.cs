namespace Infrastructure.Jwt
{
    public interface IJwtOption
    {
        string Secret { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int Expires { get; set; }
    }

    public class JwtOption : IJwtOption
    {
        public string Secret { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public int Expires { get; set; }
    }
}
