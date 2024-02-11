using System;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Objects.Tutorial.TutorialStepConditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Objects.Tutorial
{
    [Serializable]
    public class TutorialStepData
    {
        // [SFTagsGroup("TutorialSteps")]
        [SerializeField] 
        private AKTag stepTag;
        
        
        [Header("Hint data")]
        [Header("Started conditions")]
        [SerializeReference]
        [ListDrawerSettings(ShowIndexLabels = false, ListElementLabelName = "GetConditionName")]
        private ITutorialStepCondition[] startedConditions;
        
        
        [Header("Completed conditions")]
        [SerializeReference]
        [ListDrawerSettings(ShowIndexLabels = false, ListElementLabelName = "GetConditionName")]
        private ITutorialStepCondition[] completedConditions;
        
        
        [Header("Revert step conditions")]
        [SerializeReference]
        [ListDrawerSettings(ShowIndexLabels = false, ListElementLabelName = "GetConditionName")]
        private ITutorialStepCondition[] revertConditions;

        
        [Header("Analytics data")]
        [SerializeField] 
        private string analyticsName;
        
        
        public AKTag StepTag => stepTag;
        public ITutorialStepCondition[] StartConditions => startedConditions;
        public ITutorialStepCondition[] CompletedConditions => completedConditions;
        public ITutorialStepCondition[] RevertConditions => revertConditions;
        public string AnalyticsName => analyticsName;

        public string GetTutorialStepName() => $"{StepTag._Name.Split('/')[^1]} [{AnalyticsName}]";
    }
}