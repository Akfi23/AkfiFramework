﻿using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using AKFramework.Generated;
using UnityEngine;

namespace _Source.Code.Objects.Parameters
{
    [Serializable]
    public class SpeedParameterAction : IParameterAction
    {
        public AKTag ParameterTag => null;
        [SerializeField] 
        private float[] speedValue;

        public SpeedParameterAction(float[] values)
        {
            speedValue = values;
        }
        
        public float Get(int level)
        {
            return speedValue[Math.Clamp(level,0,GetMaxLevel()-1)];
        }

        public int GetMaxLevel()
        {
            return speedValue.Length;
        }
    }
}