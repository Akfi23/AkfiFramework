using _Source.Code.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(menuName = "Game/Databases/Tasks", fileName = "db_tasks")]
    public class TaskDatabase : AKDatabase
    {
        public override string Title => "Tasks Database";

        [SerializeField] 
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        private TaskData[] taskData;
        public TaskData[] TaskData => taskData;
        
#if UNITY_EDITOR

        public override void ResetData()
        {
            foreach (var taskData in TaskData)
            {
                foreach (var task in taskData.Tasks)
                {
                    // task.Reset();
                }
            }
        }
        
#endif
    }
}
