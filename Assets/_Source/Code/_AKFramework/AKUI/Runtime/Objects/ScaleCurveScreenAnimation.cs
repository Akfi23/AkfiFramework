using System;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime.Objects
{
    [Serializable]
    public class ScaleCurveScreenAnimation : IAKScreenAnimation
    {
        [SerializeField] 
        private float time;
        [SerializeField] 
        private AnimationCurve scaleCurve;
        [SerializeField] 
        private bool useUnscaledTime;
        [SerializeField]
        private RectTransform rectTransform;
        
        private float _timer = 0f;

        public async Task DoAction(CanvasGroup canvasGroup)
        {
            if (rectTransform == null)
            {
                rectTransform = canvasGroup.GetComponent<RectTransform>();
            }
            
            _timer = 0f;
            while (_timer < time)
            {
                rectTransform.localScale = Vector3.one * scaleCurve.Evaluate(_timer / time);
                _timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                await Task.Yield();
            }
        }

        public void Reset()
        {
            _timer = time;
        }
    }
}
