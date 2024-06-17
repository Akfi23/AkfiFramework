using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class DamageParameterAction : IParameterAction
    {
        public AKTag ParameterTag => null;
        [SerializeField] 
        private float[] damageValue;

        public DamageParameterAction(float[] values)
        {
            damageValue = values;
        }
        
        public float Get(int level)
        {
            return damageValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return damageValue.Length;
        }
    }
}