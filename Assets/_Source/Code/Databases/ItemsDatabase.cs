using System;
using System.Collections.Generic;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Objects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_items", menuName = "Game/Databases/Items")]
    public class ItemsDatabase : AKDatabase
    {
        public override string Title => "Items";
        
        [ListDrawerSettings(ListElementLabelName = "ItemTag")][TabGroup("Item", "ITEMS", SdfIconType.CashStack, TextColor = "green")]
        [SerializeReference]
        private List<IItem> items = new();
        public List<IItem> Items => items;

        [TabGroup("Item", "EXCHANGE DATA", SdfIconType.ArrowRepeat, TextColor = "blue")]
        [SerializeField] private ExchangeData[] exchangeDatas = new ExchangeData[]{};
        public ExchangeData[] ExchangeDatas => exchangeDatas;

#if UNITY_EDITOR
        public override void ResetData()
        {
            foreach (var item in items)
            {
                item.Init(0, 0, item.IsUnlock);
            }
        }
#endif
    }
    
    [Serializable]
    public class ExchangeData
    {
        [SerializeField] 
        private AKTag fromItemTag;
        [SerializeField]
        private AKTag toItemTag;
        [SerializeField] 
        private int pricePerOne;

        public AKTag FromItemTag => fromItemTag;
        public AKTag ToItemTag => toItemTag;
        public int PricePerOne => pricePerOne;
    }
}