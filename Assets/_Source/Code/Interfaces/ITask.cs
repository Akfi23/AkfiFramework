using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Objects;
using UnityEngine;
using Zenject;

namespace _Source.Code.Interfaces
{
    public interface ITask
    {
        Sprite GetIcon();
        string GetTitle();
        string GetDescription();
        float GetCurrentValue();
        float GetTargetValue();
        AKTag GetTargetTag();
        ItemData[] GetRewardData();
        bool HasReward();
        bool IsComplete();
        bool CanComplete();
        bool NeedAutofocusCamera();
        bool NeedArrowTargeting();

        void Init(DiContainer container, float value);
        void Start();
        void DoAction();
        void GetReward();
        void Complete();
    }
}
