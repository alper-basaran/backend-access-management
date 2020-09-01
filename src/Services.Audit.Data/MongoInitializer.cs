using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Services.Audit.Data
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;

        public MongoInitializer(IOptions<MongoOptions> options)
        {
        }

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            RegisterConventions();
            _initialized = true;
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("DefaultConventions", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}