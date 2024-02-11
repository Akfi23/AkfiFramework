using System.Threading.Tasks;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime.Interfaces
{
    public interface IAKScreenAnimation
    {
        Task DoAction(CanvasGroup canvasGroup);
        void Reset();
    }
}
