using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class HealthRefillParameterAction : IParameterAction
    {
        public AKTag ParameterTag => null;
        [SerializeField] 
        private float[] refillValue;
        
        public HealthRefillParameterAction(float[] values)
        {
            refillValue = values;
        }
        
        public float Get(int level)
        {
            return refillValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return refillValue.Length;
        }
    }
}