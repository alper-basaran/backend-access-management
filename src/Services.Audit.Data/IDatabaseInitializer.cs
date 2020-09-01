using System.Threading.Tasks;

namespace Services.Audit.Data
{
    public interface IDatabaseInitializer
    {
        void Initialize();
    }
}