using System;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime.Objects
{
    [Serializable]
    public class MoveTweenScreenAnimation : IAKScreenAnimation
    {
        [SerializeField] 
        private float time;
        [SerializeField] 
        private float delay;
        [SerializeField]
        private Vector3 endValue;
        [SerializeField]
        private Ease easeType;
        [SerializeField] 
        private bool useUnscaledTime;
        [SerializeField]
        private RectTransform rectTransform;
        
        private TaskCompletionSource<bool> _taskCompleted = new();
        
        public async Task DoAction(CanvasGroup canvasGroup)
        {
            if (rectTransform == null)
            {
                rectTransform = canvasGroup.GetComponent<RectTransform>();
            }
            
            _taskCompleted = new TaskCompletionSource<bool>();

            rectTransform.DOLocalMove(endValue, time).SetEase(easeType).SetDelay(delay).SetUpdate(useUnscaledTime)
                .OnComplete(() => _taskCompleted.TrySetResult(true));
            
            await _taskCompleted.Task;
        }

        public void Reset()
        {
            rectTransform.DOKill(true);
        }
    }
}