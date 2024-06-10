using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code.Databases;

namespace _Source.Code.Services
{
    public class MaterialService : IAKService
    {
        [AKInject]
        private MaterialDatabase _database;
        
    }
}
