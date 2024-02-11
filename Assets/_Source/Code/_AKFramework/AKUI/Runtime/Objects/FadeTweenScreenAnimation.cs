using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Code._AKFramework.AKUI.Runtime.Objects
{
    [Serializable]
    public class FadeTweenScreenAnimation : IAKScreenAnimation
    {
        [SerializeField] 
        private float time;
        [SerializeField] 
        private float delay;
        [SerializeField]
        private float endValue;
        [SerializeField]
        private Ease easeType;
        [SerializeField] 
        private bool useUnscaledTime;
        [SerializeField] 
        private Image[] images;

        private List<Tween> _tween = new();
        private TaskCompletionSource<bool> _taskCompleted = new();
        
        public async Task DoAction(CanvasGroup canvasGroup)
        {
            _taskCompleted = new TaskCompletionSource<bool>();
            if (images.Length > 0)
            {
                foreach (var image in images)
                {
                    _tween.Add(DOTween.To(() => image.color.a, x =>
                        {
                            var imageColor = image.color;
                            imageColor.a = x;
                            image.color = imageColor;
                        }, endValue, time).SetEase(easeType).SetDelay(delay)
                        .OnComplete(() => _taskCompleted.TrySetResult(true)).SetUpdate(useUnscaledTime));
                }
            }
            else
            {
                _tween.Add(DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, endValue, time).SetEase(easeType).SetDelay(delay).OnComplete(() => _taskCompleted.TrySetResult(true)).SetUpdate(useUnscaledTime));
            }
            await _taskCompleted.Task;
        }

        public void Reset()
        {
            _tween.ForEach(x => x.Kill(true));
            _tween.Clear();
        }
    }
}