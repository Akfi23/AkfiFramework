using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code._AKFramework.Data;
using _Source.Code.Interfaces;
using _Source.Code.Objects;
using AKFramework.Generated;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace _Source.Code.Data
{
    [CreateAssetMenu(menuName = "Game/Data/Task", fileName = "data_task_")]
    public class TaskData : AKData
    {
        [SerializeReference] 
        private ITask[] tasks;
        public ITask[] Tasks => tasks;
        
#if UNITY_EDITOR

        [SerializeField] 
        [FoldoutGroup("Editor")]
        private string sheetURL = "1crlFmgVGCcU_9RbUtH-Qnu4-pLmtJI5AtqGvN-2b9tU";
        [SerializeField] 
        [FoldoutGroup("Editor")]
        private string sheetNumName = "Task";
        [SerializeField] 
        [FoldoutGroup("Editor")]
        private string API_Key = "AIzaSyCzFbubPm84GJ2svQtlym34sCWYtePVaik";

        private AKTagsDatabase _tagsDatabase;
        
        [Button]
        [FoldoutGroup("Editor")]
        // private async void UpdateFromSheet()
        // {
        //     var data = Utils.Extensions.Extensions.GetSheetData<SheetFeed>(sheetURL, sheetNumName, API_Key);
        //     await data;
        //
        //     var sheetItems = new List<TaskSheetItem>();
        //     tasks = new ITask[data.Result.values.Length - 2];
        //
        //     for (int i = 2; i < data.Result.values.Length; i++)
        //     {
        //         if(data.Result.values[i] == null) continue;
        //         if(data.Result.values[i].Length <= 5) continue;
        //         if(data.Result.values[i][6] == string.Empty) continue;
        //         
        //         sheetItems.Add(new TaskSheetItem());
        //         sheetItems[i - 2].Icon = FindIcon(data.Result.values[i][0]);
        //         sheetItems[i - 2].Title = data.Result.values[i][1];
        //         sheetItems[i - 2].Description = data.Result.values[i][2];
        //         sheetItems[i - 2].TargetValue = data.Result.values[i][4];
        //         sheetItems[i - 2].Parameter = data.Result.values[i][5];
        //         sheetItems[i - 2].Tag = FindTag(data.Result.values[i][3]);
        //         if (data.Result.values[i].Length > 6)
        //         {
        //             sheetItems[i - 2].ItemData =
        //                 FindItemData(data.Result.values[i][7], data.Result.values[i][8]);
        //         } 
        //         
        //         switch (data.Result.values[i][6])
        //         {
        //             case "OpenBuilding":
        //             {
        //                 tasks[i - 2] = new UnlockBuildingTask();
        //                 break;
        //             }
        //             case "Collect":
        //             {
        //                 tasks[i - 2] = new CollectItemTask();
        //                 break;
        //             }
        //             case "Sell":
        //             {
        //                 tasks[i - 2] = new SellItemTask();
        //                 break;
        //             }
        //             case "PayBuilding":
        //             {
        //                 tasks[i - 2] = new PayBuildingTask();
        //                 break;
        //             }
        //             case "ConstructBuilding":
        //             {
        //                 tasks[i - 2] = new ConstructBuildingTask();
        //                 break; 
        //             }
        //             case "CollectItemsTotal":
        //             {
        //                 tasks[i - 2] = new CollectItemsTotalTask();
        //                 break;
        //             }
        //             case "DestroyResource":
        //             {
        //                 tasks[i - 2] = new DestroyResourceTask();
        //                 break;
        //             }
        //             case "SaveWorker":
        //             {
        //                 tasks[i - 2] = new SaveWorkerTask();
        //                 break;
        //             }
        //             case "Upgrade":
        //             {
        //                 tasks[i - 2] = new UpgradeTask();
        //                 break;
        //             }
        //         }
        //
        //         tasks[i - 2].Set(sheetItems[i - 2]);
        //     }
        // }

        private AKTag FindTag(string tagString)
        {
            if (tagString == string.Empty) return null; 
            
            if (_tagsDatabase == null)
            {
                var assets = AssetDatabase.FindAssets("db_tags");
                _tagsDatabase = AssetDatabase.LoadAssetAtPath<AKTagsDatabase>(AssetDatabase.GUIDToAssetPath(assets[0]));
            }
            
            var tagStr = tagString.Split('/');
            
            foreach (var group in _tagsDatabase.Groups)
            {
                if(!group._Name.Contains(tagStr[0])) continue;
                foreach (var tag in group.Tags)
                {
                    if (tag._Name.Contains(tagStr[1]))
                    {
                        return new AKTag(tag._Id, $"{group._Name}/{tag._Name}");
                    }
                }
            }

            return null;
        }

        private Sprite FindIcon(string spriteString)
        {
            Debug.Log(spriteString);
            var assets = AssetDatabase.FindAssets($"sprite_icon_{spriteString}");
            if (assets == null || assets.Length <= 0) return null;
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(assets[0]));

            return sprite;
        }

        private ItemData FindItemData(string tagString, string countString)
        {
            //if (tagString == null || countString == null) return new ItemData(AKTags.Player__Player, 1);
            
            return new ItemData(FindTag(tagString), int.Parse(countString));
        }

#endif
    }
}