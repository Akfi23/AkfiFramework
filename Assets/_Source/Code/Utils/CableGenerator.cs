using System;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Client_.Scripts.Utils
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteAlways]
    public class CableGenerator : MonoBehaviour
    {
        [SerializeField] 
        [OnValueChanged("Start")]
        private CableType cableType;
        public CableType CableType => cableType;

        [SerializeField] 
        private bool alwaysUpdate = true;

        [SerializeField]
        [ShowIf("cableType", CableType.Multipoint)]
        [Tooltip("Series of points in cable.  If this transform is the desired start, add it to this array.")]
        private CableSection[] cableSections;
        public CableSection[] CableSections
        {
            get => cableSections;
            set => cableSections = value;
        }
        
        [SerializeField]
        [Tooltip("Positive keys are applied downward on cable.  Recommended use is to have keys at 0 at the start and end of the curve.")]
        [ShowIf("cableType", CableType.Curve)]
        private AnimationCurve curve;

        [SerializeField]
        [Tooltip("Number of points per unit length, using the straight line from the start to the end transform.")]
        [MinValue(0.01f)]
        private float pointDensity = 3;

        [SerializeField] 
        [ShowIf("cableType", CableType.Static)]
        private float sagAmplitude = 1;

        [SerializeField, HideInInspector] 
        private Vector3 endPointPosition;
        public Vector3 EndPointPosition
        {
            get => endPointPosition;
            set => endPointPosition = value;
        }

        private LineRenderer _lineRenderer;
        private int _pointsInLineRenderer;
        private Vector3 _vectorFromStartToEnd;
        private Vector3 _sagDirection;

        private void Awake()
        {
#if !UNITY_EDITOR
            Destroy(this);
#endif
        }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _sagDirection = Physics.gravity.normalized;
        }

        private void Update()
        {
            if (!alwaysUpdate) return;
            BuildCable();
        }

        [Button]
        private void BuildCable()
        {
            switch (cableType)
            {
                case CableType.Static:
                {
                    _vectorFromStartToEnd = EndPointPosition - transform.position;
                    transform.forward = _vectorFromStartToEnd.normalized;
                    _pointsInLineRenderer = Mathf.FloorToInt(pointDensity * _vectorFromStartToEnd.magnitude);
                    _lineRenderer.positionCount = _pointsInLineRenderer;

                    var i = 0;

                    while (i < _pointsInLineRenderer)
                    {
                        var pointFoCalculates = (float)i / (_pointsInLineRenderer - 1);
                        var effectAtPointMultiplier = Mathf.Sin(pointFoCalculates * Mathf.PI);
                        var pointPosition = _vectorFromStartToEnd * pointFoCalculates;
                        var sagAtPoint = _sagDirection * sagAmplitude;

                        var currentPointsPosition =
                            transform.position +
                            pointPosition +
                            (Vector3.ClampMagnitude(sagAtPoint, sagAmplitude)) * effectAtPointMultiplier;

                        _lineRenderer.SetPosition(i, currentPointsPosition);
                        i++;
                    }

                    break;
                }
                case CableType.Multipoint:
                {
                    _lineRenderer.positionCount = 0;
                    var i = 0;
                    foreach (var section in cableSections)
                    {
                        BuildCable(section, i);
                        i++;
                    }
                    break;
                }

                case CableType.Curve:
                {
                    _vectorFromStartToEnd = EndPointPosition - transform.position;
                    transform.forward = _vectorFromStartToEnd.normalized;
                    _pointsInLineRenderer = Mathf.FloorToInt(pointDensity * _vectorFromStartToEnd.magnitude);
                    _lineRenderer.positionCount = _pointsInLineRenderer;
                    
                    int i = 0;
			   
                    while(i < _pointsInLineRenderer)
                    {
                        var pointForCalcs = (float)i / (_pointsInLineRenderer - 1);

                        var pointPosition = _vectorFromStartToEnd * pointForCalcs;
                        var sagAtPoint = _sagDirection * - curve.Evaluate(pointForCalcs);

                        var currentPointsPosition = transform.position + pointPosition + sagAtPoint;

                        _lineRenderer.SetPosition(i, currentPointsPosition);
                        i++;
                    }
                    
                    break;
                }
            }
        }

        private void BuildCable(CableSection section, int index)
        {
            var start = index == 0 ? transform.position : cableSections[index - 1].End;
            var vectorFromStartToEnd = section.End - start;
            //section.Start.forward = vectorFromStartToEnd.normalized;
            var pointsCount = Mathf.FloorToInt(pointDensity * vectorFromStartToEnd.magnitude);
            var i = 0;

            while (i < pointsCount)
            {
                var pointForCalcs = (float)i / ((pointsCount - 1) > 0 ? pointsCount - 1 : 1);
                var effectAtPointMultiplier = Mathf.Sin(pointForCalcs * Mathf.PI);
                
                var pointPosition = vectorFromStartToEnd * pointForCalcs;
                var sagAtPoint = _sagDirection * section.Sag;
                
                var currentPointsPosition = start +
                                            pointPosition +
                                            sagAtPoint * effectAtPointMultiplier;

                _lineRenderer.positionCount += 1;
                
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, currentPointsPosition);
                i++;
            }
        }
    }

    public enum CableType
    {
        Static = 0,
        Multipoint = 1,
        Curve = 2
    }
    
    [Serializable]
    public class CableSection
    {
        public Vector3 End;
        public float Sag;
    }
}