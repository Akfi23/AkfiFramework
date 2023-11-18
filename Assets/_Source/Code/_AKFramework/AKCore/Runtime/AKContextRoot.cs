using UnityEngine;
using Zenject;

namespace _Source.Code._Core.Installers
{
    public abstract class AKContextRoot : MonoInstaller
    {
        public static DiContainer RootContainer => _rootContainer;
        
        private static DiContainer _rootContainer;
    
        [SerializeField]
        private bool _debug;
        
        protected virtual void Awake()
        {
            AKDebug.SetDebug((!Application.isEditor && Debug.isDebugBuild) || (Application.isEditor && _debug));
            PreInit();

            _rootContainer = Container;
        }

        protected virtual void Start()
        {
            Init(RootContainer);
        }

        protected abstract void PreInit();
        protected abstract void Init(DiContainer container);
    }
}
