using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    [Serializable]
    public class AKTypeContainer : ISerializationCallbackReceiver
    {
        public string _Name => _name;
        public int _Id => _id;

        [SerializeField]
        private string _name = string.Empty;
        [ReadOnly]
        [SerializeField]
        private int _id = 0;

        public void OnBeforeSerialize()
        {
            if (_id != 0)
            {
                if (!AKIDGenerator.Has(_id))
                {
                    AKIDGenerator.Add(_id);
                }
                return;
            }
            
            _id = AKIDGenerator.Generate();
            AKIDGenerator.Add(_id);
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}