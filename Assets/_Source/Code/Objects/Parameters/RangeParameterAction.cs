using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class RangeParameterAction : IParameterAction
    {
        public AKTag ParameterTag => AKTags.Player__Player;
        [SerializeField] 
        private float[] rangeValue;

        public RangeParameterAction(float[] values)
        {
            rangeValue = values;
        }
        
        public float Get(int level)
        {
            return rangeValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return rangeValue.Length;
        }
    }
}