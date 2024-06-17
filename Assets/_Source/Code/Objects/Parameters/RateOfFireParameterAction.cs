using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class RateOfFireParameterAction : IParameterAction
    {
        public AKTag ParameterTag => null;

        [SerializeField]
        private float[] rateOfFireValue;

        public RateOfFireParameterAction(float[] values)
        {
            rateOfFireValue = values;
        }

        public float Get(int level)
        {
            return rateOfFireValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return rateOfFireValue.Length;
        }
    }
}