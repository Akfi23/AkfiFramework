using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Source.Code._AKFramework.AKPools.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Pools/Prefab Data")]
    [InlineEditor]
    public class AKPrefabData : AKPoolObjectData
    {
        public AssetReferenceGameObject PrefabAssetReference => _prefabAssetReference;

        [SerializeField]
        private AssetReferenceGameObject _prefabAssetReference;
    }
}