using _Source.Code.UI.Joystick;
using UnityEngine.EventSystems;

namespace _Client_.Scripts.Joystick
{
    public class FloatingJoystickView : JoystickView
    {
        protected override void Init()
        {
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
    }
}
