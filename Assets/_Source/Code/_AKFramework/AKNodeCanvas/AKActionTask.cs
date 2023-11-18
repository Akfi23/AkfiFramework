using _Source.Code._Core.Installers;
using NodeCanvas.Framework;
using Zenject;

namespace _Source.Code._AKFramework.AKNodeCanvas
{
    public abstract class AKActionTask<T> : ActionTask<T> where T : class
    {
        protected override string OnInit()
        {
            AKContextRoot.RootContainer.Inject(this);
            return base.OnInit();
        }
        
        [Inject]
        protected abstract void Init(DiContainer container);
    }
    
    public abstract class AKActionTask : ActionTask
    {
        protected override string OnInit()
        {
            AKContextRoot.RootContainer.Inject(this);
            return base.OnInit();
        }


        [Inject]
        protected abstract void Init(DiContainer container);
    }
}