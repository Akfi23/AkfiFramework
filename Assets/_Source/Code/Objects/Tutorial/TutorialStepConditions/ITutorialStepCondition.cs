using _Source.Code._AKFramework.AKCore.Runtime;
using Leopotam.EcsLite;

namespace _Source.Code.Objects.Tutorial.TutorialStepConditions
{
    public interface ITutorialStepCondition
    {
        string GetConditionName();
        bool CheckCondition(ref EcsWorld world, ref IAKContainer container);
        void Init(ref EcsWorld world, ref IAKContainer container);
        void Reset();
    }
}