using _Source.Code.Objects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_upgrades", menuName = "Game/Databases/Upgrades")]
    public class UpgradesDatabase : AKDatabase
    {
        public override string Title => "Upgrades";
        
        [TabGroup("upgrades", "UPGRADES", SdfIconType.CartCheckFill, TextColor = "yellow")]
        [SerializeField] 
        private UpgradeData[] upgradeDatas;

        [TabGroup("upgrades", "CONSTRAINTS", SdfIconType.LockFill, TextColor = "red")] 
        [SerializeField]
        private ConstraintUpgradeData[] _constraintUpgradeDatas;

        public UpgradeData[] UpgradeDatas => upgradeDatas;
        public ConstraintUpgradeData[] ConstraintUpgradeDatas => _constraintUpgradeDatas;
    }
}