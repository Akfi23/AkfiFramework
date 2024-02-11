using System;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime.Objects
{
    [Serializable]
    public class MoveCurveScreenAnimation : IAKScreenAnimation
    {
        [SerializeField] 
        private float time;
        [SerializeField]
        private Vector3 endValue;
        [SerializeField] 
        private AnimationCurve moveCurve;
        [SerializeField] 
        private bool useUnscaledTime;
        [SerializeField] 
        private RectTransform rectTransform;

        private Vector3 _startPosition;
        private float _timer = 0f;

        public async Task DoAction(CanvasGroup canvasGroup)
        {
            if (rectTransform == null)
            {
                rectTransform = canvasGroup.GetComponent<RectTransform>();
            }

            _startPosition = rectTransform.localPosition;
            _timer = 0f;
            do
            {
                _timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                rectTransform.localPosition = Vector3.Lerp(_startPosition, endValue, moveCurve.Evaluate(_timer/time));
                await Task.Yield();
            } 
            while (_timer < time);
        }

        public void Reset()
        {
            _timer = time;
        }
    }
}
