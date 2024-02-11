using System;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Objects;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_buildings", menuName = "Game/Databases/Buildings")]
    public class BuildingsDatabase : AKDatabase
    {
        public override string Title => "Buildings";

        [SerializeField] 
        private BuildingData[] buildingsData;
        public BuildingData[] BuildingsData => buildingsData;
        
#if UNITY_EDITOR
        [Button]
        public override void ResetData()
        {
            AssetDatabase.SaveAssets();
        }

        public void OnValidate()
        {
            
        }
#endif
    }

    [Serializable]
    public class BuildingData
    {
        // [AKTagsGroup("Buildings")]
        [SerializeField] [GUIColor(0.3f, 0.8f, 0.8f, 1f)][Space]
        private AKTag buildingTag;
        [SerializeField]
        private bool isUnlock;
        [SerializeField]
        [InlineProperty][HideIf("isUnlock")]
        private ItemData[] receipt;
        [SerializeField]
        private bool isBuilt;
        [SerializeField][LabelText("Time to build (In Seconds)")][HideIf("isBuilt")]
        private float timeToBuild;
        
        public AKTag BuildingTag => buildingTag;
        public bool IsUnlock => isUnlock;
        public ItemData[] Receipt => receipt;
        public bool IsBuilt => isBuilt;
        public float TimeToBuild => timeToBuild;
    }
}
