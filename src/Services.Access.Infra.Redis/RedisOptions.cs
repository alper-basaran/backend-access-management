
namespace Services.Access.Infra.RedisCache
{
    public class RedisOptions
    {
        public RedisEndpoint[] Endpoints { get; set; }
        public string Password { get; set; }
        public string Namespace { get; set; }
    }

    public class RedisEndpoint
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
