using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code.Objects.Tutorial.TutorialStepConditions
{
    [Serializable]
    public class CompleteTaskCondition : ITutorialStepCondition
    {
        private bool _init = false;
        private bool _subscribed = false;
        private bool _taskCompleted = false;
        private bool _taskFullyCompleted = false;

        private TaskService _taskService;
        
        [SerializeField] private int levelIndex;
        [SerializeField] private int taskIndex;
        [SerializeField] private bool waitRewardCollecting = true;

        public string GetConditionName() => $"Complete task: [{levelIndex}|{taskIndex}]";

        public bool CheckCondition(ref EcsWorld world, ref IAKContainer container)
        {
            if (!_init) Init(ref world, ref container);

            var taskData = _taskService.GetCurrentTaskIndex();
            var taskWasAlreadyCompleted = taskData.CurrentLevel == levelIndex && taskData.CurrentIndex > taskIndex;
            if (_taskCompleted)
            {
                _taskFullyCompleted = !waitRewardCollecting || taskWasAlreadyCompleted;
            }
            return _taskFullyCompleted || taskWasAlreadyCompleted;
        }

        public void Init(ref EcsWorld world, ref IAKContainer container)
        {
            _taskService = container.Resolve<TaskService>();
            if (!_subscribed)
            {
                _taskService.OnTaskComplete += OnTaskComplete;
                _subscribed = true;
            }

            _init = true;
        }

        private void OnTaskComplete()
        {
            var taskData = _taskService.GetCurrentTaskIndex();
            if (levelIndex != taskData.CurrentLevel || taskIndex != taskData.CurrentIndex) return;
            
            _taskCompleted = true;
        }

        public void Reset()
        {
            if (_subscribed && _taskService != null)
            {
                _taskService.OnTaskComplete -= OnTaskComplete;
                _subscribed = false;
            }
            _taskService = null;
            _init = false;
            _taskCompleted = false;
            _taskFullyCompleted = false;
        }
    }
}