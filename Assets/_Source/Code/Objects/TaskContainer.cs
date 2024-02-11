using System;
using System.Collections.Generic;

namespace _Client_.Scripts.TaskSystem.Objects
{
    [Serializable]
    public class TaskContainer
    {
        public int CurrentLevel;
        public int CurrentIndex;
        public List<TaskValue> CurrentValue;
    }

    [Serializable]
    public class TaskValue
    {
        public int Level;
        public int Index;
        public float Value;
    }
}