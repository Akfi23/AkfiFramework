using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._Core.Installers;
using NodeCanvas.Framework;

namespace _Source.Code._AKFramework.AKNodeCanvas
{
    public abstract class AKActionTask<T> : ActionTask<T>,IAKInjectable where T : class
    {
        protected override string OnInit()
        {
            AKContextRoot.Container.Inject(this);
            return base.OnInit();
        }
        
        [AKInject]
        protected abstract void Init(IAKContainer container);
    }
    
    public abstract class AKActionTask : ActionTask,IAKInjectable
    {
        protected override string OnInit()
        {
            AKContextRoot.Container.Inject(this);
            return base.OnInit();
        }


        [AKInject]
        protected abstract void Init(IAKContainer container);
    }
}