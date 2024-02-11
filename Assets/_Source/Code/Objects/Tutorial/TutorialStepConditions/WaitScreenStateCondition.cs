using System;
using _Source.Code._AKFramework.AKUI.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Source.Code.Objects.Tutorial.TutorialStepConditions
{
    [Serializable]
    public class WaitScreenStateCondition : ITutorialStepCondition
    {
        private bool _init = false;

        private IAKUIService _uiService;

        [SerializeField] private AKScreen sfScreen;
        [SerializeField] private AKScreenState[] targetStates;
        
        public string GetConditionName() => $"Wait for {sfScreen} in valid state";

        public bool CheckCondition(ref EcsWorld world, ref DiContainer container)
        {
            if (!_init) Init(ref world, ref container);

            foreach (var targetState in targetStates)
            {
                if (_uiService.GetScreenState(sfScreen) == targetState) return true;
            }

            return false;
        }

        public void Init(ref EcsWorld world, ref DiContainer container)
        {
            _uiService = container.Resolve<IAKUIService>();
            _init = true;
        }

        public void Reset()
        {
            if (_init)
            {
                _uiService = null;
                _init = false;
            }
        }
    }
}