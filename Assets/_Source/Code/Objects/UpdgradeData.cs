using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects
{
    [Serializable]
    public class UpgradeData
    {
        // [SerializeField][GUIColor("yellow")][SFTagsGroup("Upgrades")]
        private AKTag upgradeTag;
        // [SerializeField][SFTagsGroup("Items")]
        private AKTag payItemTag;
        [SerializeField][PreviewField(50, ObjectFieldAlignment.Right)]
        private Sprite icon;
        [SerializeField]
        private string name;
        [SerializeField][GUIColor("lightblue")]//[SuffixLabel(SdfIconType.Wallet)][Space]
        private int[] prices;
        [SerializeField][GUIColor("lightgreen")]//[SuffixLabel(SdfIconType.ArrowUp)][Space]
        private float[] upgradeValues;
        
        public AKTag UpgradeTag => upgradeTag;
        public AKTag PayItemTag => payItemTag;
        public Sprite Icon => icon;
        public string Name => name;
        public int[] Prices => prices;
        public float[] UpgradeValues => upgradeValues;
    }
}
