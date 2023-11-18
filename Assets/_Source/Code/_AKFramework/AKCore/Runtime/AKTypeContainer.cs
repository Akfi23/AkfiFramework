using System;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    [Serializable]
    public class AKTypeContainer : ISerializationCallbackReceiver
    {
        public string _Name => _name;

        public string _Id => _id;

        [SerializeField]
        private string _name = string.Empty;

        [HideInInspector]
        [SerializeField]
        private string _id = string.Empty;

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}