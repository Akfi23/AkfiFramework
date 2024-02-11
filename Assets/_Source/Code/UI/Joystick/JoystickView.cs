using _Source.Code._Core.View;
using _Source.Code.Enums;
using _Source.Code.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Source.Code.UI.Joystick
{
    public abstract class JoystickView : AKView, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Inject] 
        protected readonly IInputService inputService = null;

        [SerializeField] 
        protected bool noGraphicMode = false;
        
        [SerializeField] 
        private float handleRange = 1;
        [SerializeField] 
        private float deadZone = 0;
        [SerializeField] 
        private AxisOptions axisOptions = AxisOptions.Both;
        [SerializeField]
        private bool snapX = false;
        [SerializeField]
        private bool snapY = false;

        [SerializeField] 
        protected RectTransform background = null;
        [SerializeField] 
        private RectTransform handle = null;
        private RectTransform baseRect = null;

        private Canvas canvas;
        private Camera cam;

        protected override void Init()
        {
            base.Init();
            inputService.HandleRange = handleRange;
            inputService.DeadZone = deadZone;
            inputService.SnapX = snapX;
            inputService.SnapY = snapY;
            inputService.AxisOptions = axisOptions;

            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();

            if (canvas == null)
            {
                Debug.LogError("The Joystick is not placed inside a canvas");
            }

            var center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            var position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            var radius = background.sizeDelta * 0.5f;
            inputService.Input = (eventData.position - position) / (radius * canvas.scaleFactor);
            inputService.FormatInput();
            HandleInput(inputService.Input.magnitude, inputService.Input.normalized, radius, cam);
            handle.anchoredPosition = inputService.Input * radius * handleRange;
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            inputService.Input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalized, Vector2 radius, Camera camera)
        {
            inputService.HandleInput(magnitude, normalized);
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            var localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
            {
                var pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }

            return Vector2.zero;
        }
    }
}
