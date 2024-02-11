using _Source.Code.Objects.Tutorial;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_tutorial", menuName = "Game/Databases/Tutorial")]
    public class TutorialDatabase : AKDatabase
    {
        public override string Title => "Tutorial";
        
        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "GetTutorialStepName")]
        private TutorialStepData[] tutorialSteps;
        public TutorialStepData[] TutorialSteps => tutorialSteps;

        public void Reset()
        {
            foreach (var tutorialStepData in TutorialSteps)
            {
                foreach (var condition in tutorialStepData.StartConditions) condition.Reset();
                foreach (var condition in tutorialStepData.CompletedConditions) condition.Reset();
            }
        }
        
#if UNITY_EDITOR
        public override void ResetData()
        {
            Reset();
        }
#endif
    }
}