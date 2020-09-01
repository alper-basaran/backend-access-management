namespace Services.Audit.Data
{
    public class MongoOptions
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Seed { get; set; }        
    }
}