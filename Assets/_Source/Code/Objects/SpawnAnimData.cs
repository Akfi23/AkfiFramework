using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TweenType = _Client_.Scripts.Enums.TweenType;

namespace _Client_.Scripts.Objects
{
    [Serializable]
    public struct SpawnAnimData
    {
        public TweenType type;
        public Ease ease;
        public float duration;
        public float delay;

        [ShowIf("type",TweenType.Jump)]
        public int numJumps;
        [ShowIf("type",TweenType.Jump)]
        public float jumpPower;
    }
}