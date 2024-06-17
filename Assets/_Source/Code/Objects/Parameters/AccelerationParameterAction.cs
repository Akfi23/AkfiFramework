using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class AccelerationParameterAction : IParameterAction
    {
        public AKTag ParameterTag => null;
        [SerializeField] 
        private float[] accelerationValue;

        public AccelerationParameterAction(float[] values)
        {
            accelerationValue = values;
        }

        public float Get(int level)
        {
            return accelerationValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return accelerationValue.Length;
        }
    }
}