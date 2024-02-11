using System;
using System.Collections.Generic;
using _Client_.Scripts.Interfaces;
using _Client_.Scripts.Objects;
using _Client_.Scripts.TaskSystem.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Databases;
using _Source.Code.Interfaces;
using _Source.Code.Objects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Source.Code.Services
{
    public class TaskService
    {
        [Inject]
        private BuildingsService _buildingService;
        [Inject]
        private TaskDatabase _database;
        [Inject] 
        private ISaveService _saveService;

        public Action OnTaskChange = delegate {  };
        public Action<ItemData[]> OnTaskReward = delegate {  };
        public Action OnTaskComplete = delegate {  };
        public Action OnTaskUpdate = delegate { };
        
        private readonly Dictionary<(int, int), ITask> _taskMapping = new();

        private TaskContainer _container = null;

        [Inject]
        private void Init(DiContainer container)
        {
            OnTaskUpdate += OnTaskUpdateCalled;
            OnTaskComplete += OnTaskCompleteCalled;
            
            LoadService(container);
        }

        public void LoadService(DiContainer container)
        {
            _container = new TaskContainer
            {
                CurrentValue = new List<TaskValue>()
            };
            
            _container = _saveService.Load($"Tasks", _container);
            
            _taskMapping.Clear();
            
            for (var i = 0; i < _database.TaskData.Length; i++)
            {
                for (var j = 0; j < _database.TaskData[i].Tasks.Length; j++)
                {
                    var currentValue = 0f;
                    if (i + j >= _container.CurrentValue.Count)
                    {
                        _container.CurrentValue.Add(new TaskValue
                        {
                            Level = i,
                            Index = j,
                            Value = 0
                        });
                    }
                    else
                    {
                        currentValue = _container.CurrentValue.Find(x => x.Level == i && x.Index == j).Value;
                    }
                    
                    _database.TaskData[i].Tasks[j].Init(container, currentValue);
                    _taskMapping.Add((i, j), _database.TaskData[i].Tasks[j]);
                }
            }
            
            StartCurrentTask();
        }

        public void ResetTaskProgress()
        {
            _container = new TaskContainer
            {
                CurrentValue = new List<TaskValue>()
            };
            
            _saveService.Save($"Tasks", _container);
        }
        
        private async void StartCurrentTask()
        {
            await UniTask.WaitUntil(() => _buildingService.IsInitialized);
            
            StartTask();
        }
        
        private void StartTask()
        {
            if(_container.CurrentLevel == -1 || _container.CurrentIndex == -1) return;
            
            GetCurrentTask().Start();
        }

        private void OnTaskUpdateCalled()
        {
            _container.CurrentValue.Find(x => x.Level == _container.CurrentLevel && x.Index == _container.CurrentIndex)
                .Value = _taskMapping[(_container.CurrentLevel, _container.CurrentIndex)].GetCurrentValue();
            _saveService.Save("Tasks", _container);
        }

        private void OnTaskCompleteCalled()
        {
            _container.CurrentValue.Find(x => x.Level == _container.CurrentLevel && x.Index == _container.CurrentIndex)
                .Value = _taskMapping[(_container.CurrentLevel, _container.CurrentIndex)].GetCurrentValue();
            _saveService.Save($"Tasks", _container);
            
            if(!HasReward()) FindNextTask();
        }

        public bool HasTask()
        {
            return _container.CurrentLevel != -1 || _container.CurrentIndex != -1;
        }

        public ITask GetCurrentTask()
        {
            return GetTask(_container.CurrentLevel, _container.CurrentIndex);
        }
        
        public Sprite GetIcon() => GetCurrentTask().GetIcon();

        public string GetTitle() => GetCurrentTask().GetTitle();

        public string GetDescription() => GetCurrentTask().GetDescription();

        public float GetCurrentValue() => GetCurrentTask().GetCurrentValue();

        public float GetTargetValue() => GetCurrentTask().GetTargetValue();

        public AKTag GetTargetTag() => GetCurrentTask().GetTargetTag();

        public bool NeedAutofocusCamera() => GetCurrentTask().NeedAutofocusCamera();
        
        public bool IsComplete() => GetCurrentTask().IsComplete();

        public bool HasReward() => GetCurrentTask().HasReward();

        public void GetReward()
        {
            GetCurrentTask().GetReward();
            OnTaskReward.Invoke(GetCurrentTask().GetRewardData());
            FindNextTask();
        }

        public int GetTaskCount()
        {
            return _database.TaskData[_container.CurrentLevel].Tasks.Length;
        }

        public (int CurrentLevel, int CurrentIndex) GetCurrentTaskIndex() => (_container.CurrentLevel, _container.CurrentIndex);

        private void FindNextTask()
        {
            _container.CurrentIndex++;
            if (_container.CurrentIndex >= _database.TaskData[_container.CurrentLevel].Tasks.Length)
            {
                _container.CurrentLevel++;
                _container.CurrentIndex = 0;

                if (_container.CurrentLevel >= _database.TaskData.Length)
                {
                    _container.CurrentLevel = -1;
                    _container.CurrentIndex = -1;
                }
            }
            
            StartTask();
            
            OnTaskChange.Invoke();
            
            _saveService.Save($"Tasks", _container);
        }

        private ITask GetTask(int level, int index)
        {
            return _taskMapping.ContainsKey((level, index)) ? _taskMapping[(level, index)] : null;
        }
    }
}
