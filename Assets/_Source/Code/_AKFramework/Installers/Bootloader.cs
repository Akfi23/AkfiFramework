 using Cysharp.Threading.Tasks;
 using Sirenix.OdinInspector;
 using UnityEngine;
 using UnityEngine.AddressableAssets;
 using UnityEngine.ResourceManagement.AsyncOperations;
 using UnityEngine.ResourceManagement.ResourceProviders;
 using UnityEngine.SceneManagement;

 public class Bootloader : MonoBehaviour
 {
     [Required] [SerializeField]
     private AssetReference contextScene;
     private AsyncOperationHandle<SceneInstance> operation;
     
     protected async void Awake()
     {
         var currentScene = SceneManager.GetActiveScene();
            
         operation = contextScene.LoadSceneAsync(LoadSceneMode.Additive);
         await operation.Task;
            
         // ISFContainer container = null;
         //
         // while (container == null)
         // {
         //     container = SFContextRoot.Container;
         //     await UniTask.NextFrame();
         // }
         
         await UniTask.NextFrame();
         await SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
     }
 }
