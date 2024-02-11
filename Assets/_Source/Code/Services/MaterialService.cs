using _Source.Code.Databases;
using Zenject;

namespace _Source.Code.Services
{
    public class MaterialService
    {
        [Inject]
        private MaterialDatabase _database;
        
    }
}
