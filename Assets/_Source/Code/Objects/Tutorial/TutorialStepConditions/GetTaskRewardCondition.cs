using System;
using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Source.Code.Objects.Tutorial.TutorialStepConditions
{
    [Serializable]
    public class GetTaskRewardCondition : ITutorialStepCondition
    {
        private bool _init = false;
        private bool _subscribed = false;
        private bool _taskRewardClaimed = false;

        private TaskService _taskService;
        
        [SerializeField] private int levelIndex;
        [SerializeField] private int taskIndex;

        public string GetConditionName() => $"Get task reward: [{levelIndex}|{taskIndex}]";

        public bool CheckCondition(ref EcsWorld world, ref IAKContainer container)
        {
            if (!_init) Init(ref world, ref container);

            var taskData = _taskService.GetCurrentTaskIndex();
            return _taskRewardClaimed || (taskData.CurrentLevel == levelIndex && taskData.CurrentIndex > taskIndex);
        }

        public void Init(ref EcsWorld world, ref IAKContainer container)
        {
            _taskService = container.Resolve<TaskService>();
            if (!_subscribed)
            {
                _taskService.OnTaskReward += OnTaskReward;
                _subscribed = true;
            }
            _init = true;
        }

        private void OnTaskReward(ItemData[] data)
        {
            var taskData = _taskService.GetCurrentTaskIndex();
            if (levelIndex != taskData.CurrentLevel || taskIndex != taskData.CurrentIndex) return;
            
            _taskRewardClaimed = true;
        }

        public void Reset()
        {
            if (_subscribed && _taskService != null)
            {
                _taskService.OnTaskReward -= OnTaskReward;
                _subscribed = false;
            }
            _taskService = null;
            _init = false;
            
            _taskRewardClaimed = false;
        }
    }
}