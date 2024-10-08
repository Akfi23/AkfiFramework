using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine;

namespace _Source.Code._Core.Installers
{
    public abstract class AKContextRoot : MonoBehaviour
    {
        public static IAKContainer Container => _container;

        private static IAKContainer _container;

        [SerializeField]
        private bool _debug;
        
        protected virtual void Awake()
        {
            AKDebug.SetDebug((!Application.isEditor && Debug.isDebugBuild) || (Application.isEditor && _debug));
            _container = new AKContainer();
            PreInit();
            Setup(_container);
            _container.Inject();
        }

        protected virtual void Start()
        {
            Init(_container);
        }

        protected abstract void PreInit();
        protected abstract void Setup(IAKContainer container);
        protected abstract void Init(IAKContainer container);
    }
}
