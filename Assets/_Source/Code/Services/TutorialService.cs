using System;
using _Client_.Scripts.Interfaces;
using _Source.Code._AKFramework.AKEvents.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using _Source.Code.Objects.Tutorial;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Source.Code.Services
{
    public class TutorialService
    {
        [Inject]
        private ISaveService _saveService;
        [Inject] 
        private IAKEventsService _eventsService;
        [Inject]
        private IAKUIService _uiController;
        // [Inject] 
        // private ISFAnalyticsService _analyticsService;

        [Inject]
        private TutorialDatabase _tutorialDatabase;
        
        private int _currentTutorialStepIndexIndex = 0;
        public int CurrentTutorialStepIndex
        {
            get => _currentTutorialStepIndexIndex;
            private set
            {
                _currentTutorialStepIndexIndex = value;
                _saveService.Save("CurrentTutorialStepIndex", value);
            }
        }

        public bool CurrentTutorialStepStarted { get; private set; } = false;

        public event Action<AKTag> StepChanged;

        [Inject]
        private void Init()
        {
            LoadData();
        }

        public void LoadData()
        {
            _currentTutorialStepIndexIndex = _saveService.Load("CurrentTutorialStepIndex", 0);
            CurrentTutorialStepStarted = false;
            _tutorialDatabase.Reset();
        }
        
        public void CheckCurrentTutorialStepStarted(ref EcsWorld world, ref DiContainer container)
        {
            if (CurrentTutorialStepIndex < 0) return;
            if (CurrentTutorialStepStarted) return;
         
            
            var started = true;
            foreach (var startedCondition in GetCurrentTutorialStepData().StartConditions)
            {
                if (!startedCondition.CheckCondition(ref world, ref container)) started = false;
                if (!started) break;
            }

            if (started) StartCurrentTutorialStep();
        }
        
        private void StartCurrentTutorialStep()
        {
            CurrentTutorialStepStarted = true;
            
            StepChanged?.Invoke(GetCurrentTutorialStepData().StepTag);
        }
        
        public void CheckCurrentTutorialStepCompleted(ref EcsWorld world, ref DiContainer container)
        {
            if (CurrentTutorialStepIndex < 0) return;
            if (!CurrentTutorialStepStarted) return;

            
            var completed = true;
            foreach (var completedCondition in GetCurrentTutorialStepData().CompletedConditions)
            {
                if (!completedCondition.CheckCondition(ref world, ref container)) completed = false;
                if (!completed) break;
            }

            if (completed) CompleteCurrentTutorialStep();
        }
        
        private void CompleteCurrentTutorialStep()
        {
            if (!CurrentTutorialStepStarted) return;
            

            var completedTutorialStepIndex = CurrentTutorialStepIndex;

            ResetTutorialStepConditions(GetTutorialStepData(completedTutorialStepIndex));
            
            if (CurrentTutorialStepIndex < _tutorialDatabase.TutorialSteps.Length)
                CurrentTutorialStepIndex++;
            
            if (CurrentTutorialStepIndex >= _tutorialDatabase.TutorialSteps.Length) 
                CurrentTutorialStepIndex = -1;

            CurrentTutorialStepStarted = false; 
        }
        
        public void CheckCurrentTutorialStepRevert(ref EcsWorld world, ref DiContainer container)
        {
            if (CurrentTutorialStepIndex < 0) return;
            if (!CurrentTutorialStepStarted) return;

            
            var revert = false;
            foreach (var revertCondition in GetCurrentTutorialStepData().RevertConditions)
            {
                if (revertCondition.CheckCondition(ref world, ref container)) revert = true;
                if (revert) break;
            }

            if (revert) RevertCurrentTutorialStep();
        }
        
        private void RevertCurrentTutorialStep()
        {
            if (!CurrentTutorialStepStarted) return;

            CurrentTutorialStepStarted = false;
        }

        public void ResetTutorialStepConditions(TutorialStepData stepData)
        {
            foreach (var condition in stepData.StartConditions) condition.Reset();
            foreach (var condition in stepData.CompletedConditions) condition.Reset();
            foreach (var condition in stepData.RevertConditions) condition.Reset();
        }
        
        public TutorialStepData GetCurrentTutorialStepData() => GetTutorialStepData(CurrentTutorialStepIndex);

        private TutorialStepData GetTutorialStepData(int index)
        {
            return _tutorialDatabase.TutorialSteps[Mathf.Clamp(index, 0, _tutorialDatabase.TutorialSteps.Length - 1)];
        }

        public void CompleteTutorial()
        {
            CurrentTutorialStepStarted = false;
            CurrentTutorialStepIndex = -1;
        }
    }
}