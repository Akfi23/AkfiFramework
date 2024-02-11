using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class HealthParameterAction : IParameterAction
    {
        public AKTag ParameterTag => AKTags.Player__Player;
        [SerializeField] 
        private float[] healthValue;

        public HealthParameterAction(float[] values)
        {
            healthValue = values;
        }
        
        public float Get(int level)
        {
            return healthValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return healthValue.Length;
        }
    }
}
