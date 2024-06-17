using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class AngularSpeedParameterAction : IParameterAction
    {
        public AKTag ParameterTag => null;
        [SerializeField] 
        private float[] angularSpeedValue;

        public AngularSpeedParameterAction(float[] values)
        {
            angularSpeedValue = values;
        }
        
        public float Get(int level)
        {
            return angularSpeedValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return angularSpeedValue.Length;
        }
    }
}