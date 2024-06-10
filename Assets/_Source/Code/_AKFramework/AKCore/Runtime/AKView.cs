using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._Core.Installers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Source.Code._Core.View
{
    public abstract class AKView : MonoBehaviour, IAKInjectable
    {
        [AKInject] private IAKContainer _container;
        
        protected virtual void Awake()
        {
            PreInit();
        }

        protected virtual void Start()
        {
            if (_container == null)
                AKContextRoot.Container.Inject(this);
        }

        protected virtual void PreInit()
        {
        }

        [AKInject]
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
