namespace DotnetJwtAuth.Helpers
{
    public class TokenConfiguration
    {
        public string Secret { get; set; }
        public int TimeToExpire { get; set; }
    }
}