using _Source.Code._Core.Installers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Source.Code._Core.View
{
    public abstract class AKView : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        
        protected virtual void Awake()
        {
            PreInit();
        }

        protected virtual void Start()
        {
            if (_container == null)
                AKContextRoot.RootContainer.Inject(this);
        }

        protected virtual void PreInit()
        {
        }

        [Inject]
        private async void _inject()
        {
            Init();
            await UniTask.Yield();
            PostInit();
        }
        
        protected virtual void Init()
        {
        }

        protected virtual void PostInit()
        {
        }
    }
}
