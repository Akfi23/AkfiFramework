using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Objects;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_vibrations", menuName = "Game/Databases/Vibrations Database", order = 1)]
    public class VibrationsDatabase : AKDatabase
    {
        public override string Title => "Vibrations Database";

        [SerializeField]
        private VibrationData[] vibrationsData;

        public VibrationData GetVibrationData(AKTag tag)
        {
            foreach (var vibrationData in vibrationsData)
            {
                if (vibrationData.Tag.HasTag(tag)) return vibrationData;
            }

            return new VibrationData();
        }
    }
}