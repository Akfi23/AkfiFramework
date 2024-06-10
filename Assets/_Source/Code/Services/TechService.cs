using System;
using _Client_.Scripts.Interfaces;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code.Interfaces;

namespace _Source.Code.Services
{
    public class TechService : IAKService
    {
        [AKInject] 
        private ISaveService _saveService;

        public bool IsFirstLaunch { get; private set; }
        
        public int LaunchCount { get; private set;}
        public int LevelCount { get; private set; }
        public int DaysSinceReg { get; private set; }
        
        public int PlayTimeBeforeDie { get; private set; } = 0;

        public int TotalPlayMinutes { get; private set; } = 0;
        public int TotalPlaySeconds { get; private set; } = 0;
        
        public int LevelNumber { get; set; } = 0;
        public string LevelName { get; set; } = string.Empty;
        
        public float GameTime { get; private set; } = 0f;
        public float PrevGameTime { get; private set; } = 0f;
        
        private bool _isPause = false;
        private long _firstLaunchTime;

        
        [AKInject]
        private void Init()
        {
            IsFirstLaunch = _saveService.Load("FirstLaunch", true);

            LevelCount = _saveService.Load("LevelCount", 0);
            LaunchCount = _saveService.Load("LaunchCount", 0);

            TotalPlayMinutes = _saveService.Load("PlayMinutes", 0);
            TotalPlaySeconds = _saveService.Load("PlaySeconds", 0);

            GameTime = _saveService.Load("GameTime", 0f);
            PrevGameTime = GameTime;
            
            IncreaseLaunchCount();
            
            CalculateDaysFromFirstLaunch();

            _saveService.Save("FirstLaunch", false);
        }

        public void CalculateDaysFromFirstLaunch()
        {
            if (!_saveService.Has("FirstLaunchTime"))
            {
                _saveService.Save("FirstLaunchTime", DateTime.Now.ToBinary());
                _firstLaunchTime = DateTime.Now.ToBinary();
            }
            else
            {
                long timestamp = _saveService.Load("FirstLaunchTime",DateTime.Now.ToBinary());
                DateTime.FromBinary(Convert.ToInt64(timestamp));
                _firstLaunchTime = timestamp;
            }
            
            TimeSpan elapsedTime = DateTime.Now - DateTime.FromBinary(_firstLaunchTime);
            AKDebug.Log("FIRST lAUNCH TIME " + DateTime.FromBinary(_firstLaunchTime).ToString());
            DaysSinceReg = (int)elapsedTime.TotalDays;
        }
        
        public void IncreasePlayTimeMinute()
        {
            TotalPlayMinutes++;
            _saveService.Save("PlayMinutes",TotalPlayMinutes);
        }

        public void IncreasePlayTimeSeconds()
        {
            TotalPlaySeconds++;
            _saveService.Save("PlaySeconds",TotalPlaySeconds);
        }

        public void IncreaseLevelCount()
        {
            LevelCount++;
            _saveService.Save("LevelCount", LevelCount);
        }

        public void IncreaseLaunchCount()
        {
            LaunchCount++;
            _saveService.Save("LaunchCount",LaunchCount);
        }
        
        public void UpdateGameTimer(float deltaTime)
        {
            if (_isPause) return;

            GameTime += deltaTime;
            if ((GameTime - PrevGameTime) < 5f) return;

            PrevGameTime = GameTime;
            
            _saveService.Save("GameTime", GameTime);
        }
        
        public void OnPause(bool state)
        {
            _isPause = state;
            _saveService.Save("GameTime", GameTime);
        }
    }
}
