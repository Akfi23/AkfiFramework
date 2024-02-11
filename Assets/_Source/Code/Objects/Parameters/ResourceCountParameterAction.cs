using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class ResourceCountParameterAction : IParameterAction
    {
        public AKTag ParameterTag => AKTags.Player__Player;

        [SerializeField] 
        private float[] countValue;

        public ResourceCountParameterAction(float[] values)
        {
            countValue = values;
        }
        
        public float Get(int level)
        {
            return countValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return countValue.Length;
        }
    }
}