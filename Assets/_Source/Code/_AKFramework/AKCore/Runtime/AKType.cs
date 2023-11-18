using System;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    [Serializable]
    public abstract class AKType
    {
        public string _Id => id;

        public string _Name => name;
        
        [SerializeField, HideInInspector]
        private string name;
    
        [SerializeField, HideInInspector]
        private string id;
    
        protected AKType(string _id, string _name = AKConstants.NONE)
        {
            id = _id;
            name = _name;
        }

        public T FromContainer<T>(AKTypeContainer container) where T : AKType
        {
            id = container._Id;
            name = container._Name;
            return this as T;
        }

        protected AKType()
        {
            id = Guid.NewGuid().ToString();
            name = AKConstants.NONE;
        }

        protected bool Equals(AKType other)
        {
            return Equals(id, other.id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != GetType()) return false;
            return Equals((AKType)obj);
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(id)) return 0;
            return id.GetHashCode();
        }

        public override string ToString()
        {
            return name;
        }

        public static bool operator ==(AKType a, AKType b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.id.Equals(b.id);
        }

        public static bool operator !=(AKType a, AKType b)
        {
            return !(a == b);
        }

        public bool IsNone => string.IsNullOrWhiteSpace(id);

        public virtual void Reset()
        {
            id = string.Empty;
        }
    }
}