using _Client_.Scripts.Enums;
using _Source.Code.Enums;
using _Source.Code.Interfaces;
using UnityEngine;

namespace _Source.Code.Services
{
    public class InputService : IInputService
    {
        public bool IsActive { get; private set; } = true;
        public float Horizontal => (SnapX) ? SnapFloat(Input.x, AxisOptions.Horizontal) : Input.x;
        public float Vertical => (SnapY) ? SnapFloat(Input.y, AxisOptions.Vertical) : Input.y;
        public Vector2 Direction => new(Horizontal, Vertical);

        public bool SnapX { get; set; }
        public bool SnapY { get; set; }
        public AxisOptions AxisOptions { get; set; }

        private float _handleRange;
        public float HandleRange
        {
            get => _handleRange;
            set => _handleRange = Mathf.Abs(value);
        }

        private float _deadZone;
        public float DeadZone
        {
            get => _deadZone;
            set => _deadZone = Mathf.Abs(value);
        }

        private float _moveThreshold;
        public float MoveThreshold
        {
            get => _moveThreshold;
            set => _moveThreshold = Mathf.Abs(value);
        }

        private Vector2 _input = Vector2.zero;
        public Vector2 Input
        {
            get => IsActive ? _input : Vector2.zero;
            set => _input = value;
        }

        public void SetActive(bool value)
        {
            IsActive = value;
        }

        public void FormatInput()
        {
            var vector2 = Input;
            switch (AxisOptions)
            {
                case AxisOptions.Horizontal:
                {
                    vector2.y = 0f;
                    break;
                }

                case AxisOptions.Vertical:
                {
                    vector2.x = 0f;
                    break;
                }
            }

            Input = vector2;
        }

        public void HandleInput(float magnitude, Vector2 normalized)
        {
            if (IsActive)
            {
                if (magnitude > _deadZone)
                {
                    if (magnitude > 1)
                    {
                        Input = normalized;
                    }
                }
                else
                {
                    Input = Vector2.zero;
                }
            }
            else
            {
                Input = Vector2.zero;
            }
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (AxisOptions == AxisOptions.Both)
            {
                var angle = Vector2.Angle(Input, Vector2.up);
                switch (snapAxis)
                {
                    case AxisOptions.Horizontal when angle < 22.5f || angle > 157.5f:
                        return 0;
                    case AxisOptions.Horizontal:
                        return (value > 0) ? 1 : -1;
                    case AxisOptions.Vertical when angle > 67.5f && angle < 112.5f:
                        return 0;
                    case AxisOptions.Vertical:
                        return (value > 0) ? 1 : -1;
                    default:
                        return value;
                }
            }

            switch (value)
            {
                case > 0:
                    return 1;
                case < 0:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}