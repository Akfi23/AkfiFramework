using System;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Code._AKFramework.AKUI.Runtime.Objects
{
    [Serializable]
    public class FadeCurveScreenAnimation : IAKScreenAnimation
    {
        [SerializeField] 
        private float time;
        [SerializeField] 
        private AnimationCurve fadeCurve;
        [SerializeField] 
        private bool useUnscaledTime;
        [SerializeField] 
        private Image[] images;

        private float _timer = 0f;
        private Color _imageColor;
        
        public async Task DoAction(CanvasGroup group)
        {
            _timer = 0f;
            while (_timer < time)
            {
                if (images.Length > 0)
                {
                    foreach (var image in images)
                    {
                        _imageColor = image.color;
                        _imageColor.a = fadeCurve.Evaluate(_timer / time);
                        image.color = _imageColor;
                    }
                }
                else
                {
                    group.alpha = fadeCurve.Evaluate(_timer / time);
                }

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