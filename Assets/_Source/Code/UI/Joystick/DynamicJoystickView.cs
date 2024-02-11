using _Source.Code.UI.Joystick;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Client_.Scripts.Joystick
{
    public class DynamicJoystickView : JoystickView
    {
        [SerializeField] 
        private float moveThreshold = 1;

        protected override void Init()
        {
            inputService.MoveThreshold = moveThreshold;
            base.Init();
            background.gameObject.SetActive(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(!inputService.IsActive) return;
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(!noGraphicMode);
            base.OnPointerDown(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (inputService.IsActive && !background.gameObject.activeSelf)
            {
                background.gameObject.SetActive(!noGraphicMode);
            }
            base.OnDrag(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalized, Vector2 radius, Camera cam)
        {
            if(!inputService.IsActive) return;
            if (magnitude > inputService.MoveThreshold)
            {
                var difference = normalized * (magnitude - inputService.MoveThreshold) * radius;
                background.anchoredPosition += difference;
            }
            base.HandleInput(magnitude, normalized, radius, cam);
        }
    }
}
