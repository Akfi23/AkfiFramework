using _Source.Code.Enums;
using UnityEngine;

namespace _Source.Code.Interfaces
{
    public interface IInputService
    {
        public bool IsActive { get; }
        public float Horizontal { get; }
        public float Vertical { get; }
        public Vector2 Direction { get; }
        public float HandleRange { get; set; }
        public float DeadZone { get; set; }
        public AxisOptions AxisOptions { get; set; }
        public bool SnapX { get; set; }
        public bool SnapY { get; set; }
        public Vector2 Input { get; set; }
        public float MoveThreshold { get; set; }

        void FormatInput();
        void HandleInput(float magnitude, Vector2 normalized);
        void SetActive(bool value);
    }
}
