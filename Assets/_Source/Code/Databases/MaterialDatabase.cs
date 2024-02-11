using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_materials", menuName = "Game/Databases/Materials")]
    public class MaterialDatabase : AKDatabase
    {
        public override string Title => "Materials";
        
    }
}
