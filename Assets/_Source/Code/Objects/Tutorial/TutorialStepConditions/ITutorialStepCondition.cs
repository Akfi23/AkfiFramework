using Leopotam.EcsLite;
using Zenject;

namespace _Source.Code.Objects.Tutorial.TutorialStepConditions
{
    public interface ITutorialStepCondition
    {
        string GetConditionName();
        bool CheckCondition(ref EcsWorld world, ref DiContainer container);
        void Init(ref EcsWorld world, ref DiContainer container);
        void Reset();
    }
}